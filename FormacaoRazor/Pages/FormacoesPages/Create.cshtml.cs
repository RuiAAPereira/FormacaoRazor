using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FormacaoRazor.Models;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;

namespace FormacaoRazor.Pages.FormacoesPages
{
    [Breadcrumb("Criar", FromPage = typeof(IndexModel))]
    [Authorize(Roles = "Admin, Administrativo")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public CreateModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Formacao Formacao { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Formacoes.Add(Formacao);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Index").WithSuccess("OK!", "Formação adicionada com sucesso.");

        }
    }
}