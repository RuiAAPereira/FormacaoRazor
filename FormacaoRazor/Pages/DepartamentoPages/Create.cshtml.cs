using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.DepartamentoPages
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
            ViewData["UhId"] = new SelectList(db.Uhs, "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");
            return Page();
        }

        [BindProperty]
        public Departamento Departamento { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Departamentos.Add(Departamento);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Index").WithSuccess("OK!", "Departamento adicionado com sucesso.");
        }
    }
}