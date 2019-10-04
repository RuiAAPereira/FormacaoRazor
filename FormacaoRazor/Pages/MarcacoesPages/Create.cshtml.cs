using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using System;
using FormacaoRazor.Extensions.Alerts;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Criar", FromPage = typeof(IndexModel))]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public CreateModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult OnGet()
        {
            ViewData["FormacaoCod"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoCod");
            ViewData["FormacaoNome"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoNome");
            ViewData["FormadorID"] = new SelectList(db.Formadores, "FormadorID", "FormadorNome");
            ViewData["SalaId"] = new SelectList(db.Salas, "SalaID", "SalaNome");
            ViewData["UhId"] = new SelectList(db.Uhs, "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");
            return Page();
        }

        [BindProperty]
        public Marcacao Marcacao { get; set; }
        private string HoraInicio { get; set; }
        private string HoraFim { get; set; }
        //private List<DateTime> Dates { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            Guid formacaoId = new Guid(Request.Form["Marcacao.FormacaoId"]);
            Guid formadorId = new Guid(Request.Form["Marcacao.FormadorID"]);

            string h = (string)Request.Form["customRadio"];
            switch (h)
            {
                case "TodoDia":
                    HoraInicio = "09:00:00";
                    HoraFim = "17:30:00";
                    break;
                case "Manha":
                    HoraInicio = "09:00:00";
                    HoraFim = "13:00:00";
                    break;
                case "Tarde":
                    HoraInicio = "14:00:00";
                    HoraFim = "18:00:00";
                    break;
            }

            string temp = Request.Form["customRadio"] == "TodoDia" ? "on" : (string)Request.Form["customRadio"];

            Marcacao.ColorCode = db.Formacoes
                .Where(i => i.FormacaoId == formacaoId)
                .Select(c => c.FormacaoColor).Single();

            int duracao = db.Formacoes
                .Where(i => i.FormacaoId == formacaoId)
                .Select(c => c.FormacaoDuracao).Single();

            Marcacao.DataInicio = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraInicio, CultureInfo.GetCultureInfo("pt-PT"));
            DateTime dataFim = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraFim, CultureInfo.GetCultureInfo("pt-PT"));

            Marcacao.DataFim =  dataFim.AddDays(duracao - 1);

            List<Marcacao> formadorOcupado = db.Marcacoes
                .Where(d => d.FormadorID == formadorId
                && Marcacao.DataInicio <= d.DataFim
                && Marcacao.DataFim >= d.DataInicio)
                 .Include(m => m.Formacao)
                .ToList();

            string message = string.Empty;

            foreach (Marcacao item in formadorOcupado)
            {
                message += " ";
                message += item.Formacao.FormacaoCod;
                message += " ";
                message += item.DataInicio;
                message += "-";
                message += item.DataFim;
            }

            if (formadorOcupado.Any())
            {
                ModelState.AddModelError("", "O formador está ocupado" + message);
                return OnGet();
            }

                //_ = db.Marcacoes.Add(Marcacao);
                //_ = await db.SaveChangesAsync().ConfigureAwait(true);

                return RedirectToPage("Index").WithSuccess("OK!", formadorOcupado.ToString());

                //return RedirectToPage("Index").WithSuccess("OK!", "Marcação adicionada com sucesso.");
            

           
        }

    }
}