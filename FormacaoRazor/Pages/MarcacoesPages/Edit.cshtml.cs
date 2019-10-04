using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;
using System.Globalization;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Editar", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Marcacao Marcacao { get; set; }
        private string HoraInicio { get; set; }
        private string HoraFim { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Guid formacaoId = new Guid(Request.Form["Marcacao.FormacaoId"]);

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

            Marcacao.DataFim = dataFim.AddDays(duracao - 1);

            db.Attach(Marcacao).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarcacaoExists(Marcacao.MarcacaoID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("Index").WithInfo("OK!", "Marcação alterada com sucesso.");
        }

        private bool MarcacaoExists(Guid id)
        {
            return db.Marcacoes.Any(e => e.MarcacaoID == id);
        }
    }
}
