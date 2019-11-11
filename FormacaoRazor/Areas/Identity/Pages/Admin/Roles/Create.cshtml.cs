using System.Globalization;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.Roles
{
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
        public ApplicationRole ApplicationRole { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string name = Request.Form["ApplicationRole.Name"];
            ApplicationRole.NormalizedName = name.ToUpper(CultureInfo.CurrentCulture);

            _ = db.ApplicationRoles.Add(ApplicationRole);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Index").WithSuccess("OK!", "Função adicionada com sucesso.");
        }
    }
}
