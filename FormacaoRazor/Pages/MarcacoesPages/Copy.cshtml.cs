using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Models.Marcacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Copiar", FromPage = typeof(IndexModel))]
    [Authorize(Roles = "Admin, Administrativo")]
    public class CopyModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public CopyModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Marcacao Marcacao { get; set; }
        private string HoraInicio { get; set; }
        private string HoraFim { get; set; }
        [BindProperty]
        public int Duracao { get; set; }
        [BindProperty]
        public Guid FormacaoId2 { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Marcacao = await db.Marcacoes
                .Include(m => m.Formacao)
                .Include(m => m.Formador)
                .Include(m => m.Sala)
                .Include(m => m.Uh).FirstOrDefaultAsync(m => m.MarcacaoID == id).ConfigureAwait(false);

            if (Marcacao == null)
            {
                return NotFound();
            }
            ViewData["FormacaoCod"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoCod");
            ViewData["FormacaoNome"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoNome");
            ViewData["FormadorID"] = new SelectList(db.Formadores, "FormadorID", "FormadorNome");
            ViewData["SalaId"] = new SelectList(db.Salas, "SalaID", "SalaNome");
            ViewData["UhId"] = new SelectList(db.Uhs, "UhId", "IATA");

            FormacaoId2 = Marcacao.FormacaoId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Guid formacaoId = new Guid(Request.Form["Marcacao.FormacaoId"]);
            Guid formadorId = new Guid(Request.Form["Marcacao.FormadorID"]);
            Guid salaId = new Guid(Request.Form["Marcacao.SalaId"]);
            var externosList = new List<Guid>();
            externosList.AddRange(new[] {
                    new Guid("a3a35247-797a-4933-aec1-6eef7f3d9961"),
                    new Guid("e28b0c7f-cada-4ed6-b658-2733aec99833")
            });

            string message = string.Empty;

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
                default:
                    break;
            }

            string temp = Request.Form["customRadio"] == "TodoDia" ? "on" : (string)Request.Form["customRadio"];

            Marcacao.ColorCode = db.Formacoes
                .Where(i => i.FormacaoId == formacaoId)
                .Select(c => c.FormacaoColor).Single();

            int duracao;
            duracao = Convert.ToInt32(Request.Form["Duracao"], CultureInfo.CurrentCulture) == 0
                ? db.Formacoes
                    .Where(i => i.FormacaoId == formacaoId)
                    .Select(c => c.FormacaoDuracao).Single()
                : Convert.ToInt32(Request.Form["Duracao"], CultureInfo.CurrentCulture);

            Marcacao.DataInicio = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraInicio, CultureInfo.GetCultureInfo("pt-PT"));
            DateTime dataFim = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraFim, CultureInfo.GetCultureInfo("pt-PT"));

            Marcacao.DataFim = dataFim.AddDays(duracao - 1);

            List<Marcacao> formadorOcupado = db.Marcacoes
            .Where(d => d.FormadorID == formadorId
            && Marcacao.DataInicio <= d.DataFim
            && Marcacao.DataFim >= d.DataInicio
            && !externosList.Contains(d.FormadorID))
             .Include(m => m.Formacao)
            .ToList();

            foreach (Marcacao item in formadorOcupado)
            {
                message += " ";
                message += item.Formacao.FormacaoCod;
                message += " ";
                message += item.DataInicio;
                message += " - ";
                message += item.DataFim;
            }

            if (formadorOcupado.Any())
            {
                ModelState.AddModelError("", "O formador está ocupado" + message);
                return await OnGetAsync(id).ConfigureAwait(false);
            }

            List<Marcacao> salaOcupada = db.Marcacoes
                .Where(s => s.SalaId == salaId
                && Marcacao.DataInicio <= s.DataFim
                && Marcacao.DataFim >= s.DataInicio)
                .Include(m => m.Formacao)
                .ToList();

            foreach (Marcacao item in salaOcupada)
            {
                message += " ";
                message += item.Formacao.FormacaoCod;
                message += " ";
                message += item.DataInicio;
                message += " - ";
                message += item.DataFim;
            }

            if (salaOcupada.Any())
            {
                ModelState.AddModelError("", "A sala está ocupada" + message);
                return await OnGetAsync(id).ConfigureAwait(false);
            }
            Marcacao.MarcacaoID = new Guid();
            _ = db.Marcacoes.Add(Marcacao);
            _ = await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Index").WithSuccess("OK!", "Marcação adicionada com sucesso.");

        }

    }
}