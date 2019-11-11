using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System.Globalization;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Marcações")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Marcacao> MarcacaoList { get;set; }
        [BindProperty]
        public Marcacao Marcacao { get; set; }
        private string HoraInicio { get; set; }
        private string HoraFim { get; set; }
        [BindProperty]
        public Guid FormacaoId2 { get; set; }

        public async Task OnGetAsync()
        {
            MarcacaoList = await db.Marcacoes
                .Include(m => m.Formacao)
                .Include(m => m.Formador)
                .Include(m => m.Sala)
                .Include(m => m.Uh)
                .OrderByDescending(d=>d.DataInicio)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public PartialViewResult OnGetCopyModalPartial(Guid? id)
        {

            Marcacao = db.Marcacoes
                .Include(m => m.Formacao)
                .Include(m => m.Formador)
                .Include(m => m.Sala)
                .Include(m => m.Uh).FirstOrDefault(m => m.MarcacaoID == id);

            ViewData["FormacaoCod"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoCod");
            ViewData["FormacaoNome"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoNome");
            ViewData["FormadorID"] = new SelectList(db.Formadores, "FormadorID", "FormadorNome");
            ViewData["SalaId"] = new SelectList(db.Salas, "SalaID", "SalaNome");
            ViewData["UhId"] = new SelectList(db.Uhs, "UhId", "IATA");

            FormacaoId2 = Marcacao.FormacaoId;

            return new PartialViewResult
            {
                ViewName = "_CopyModalPartial",
                ViewData = new ViewDataDictionary<Marcacao>(ViewData, Marcacao)
            };
        }

        public PartialViewResult OnPostAddModalPartial(Marcacao model)
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Guid> userUhs = db.UsersUhs.Where(i => i.User.Id == uId).Select(u => u.UhId).ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)), "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            Guid formacaoId = new Guid(Request.Form["Marcacao.FormacaoId"]);
            Guid formadorId = new Guid(Request.Form["Marcacao.FormadorID"]);
            Guid salaId = new Guid(Request.Form["Marcacao.SalaId"]);

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

            int duracao = db.Formacoes
                .Where(i => i.FormacaoId == formacaoId)
                .Select(c => c.FormacaoDuracao).Single();

            Marcacao.DataInicio = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraInicio, CultureInfo.GetCultureInfo("pt-PT"));
            DateTime dataFim = DateTime.Parse(Request.Form["Marcacao-DataInicio"] + " " + HoraFim, CultureInfo.GetCultureInfo("pt-PT"));

            Marcacao.DataFim = dataFim.AddDays(duracao - 1);

            if (ModelState.IsValid)
            {
                if (model != null)
                {

                    _ = db.Marcacoes.Add(model);
                    _ = db.SaveChangesAsync().ConfigureAwait(true);
                }
            }

            return new PartialViewResult
            {
                ViewName = "_CopyModalPartial",
                ViewData = new ViewDataDictionary<Marcacao>(ViewData, model)
            };

        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Marcacao = await db.Marcacoes.FindAsync(new Guid(id)).ConfigureAwait(false);

            if (Marcacao != null)
            {
                _ = db.Marcacoes.Remove(Marcacao);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Marcação apagada.");
        }
    }
}
