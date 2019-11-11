using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.Roles
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationRole = await db.ApplicationRoles.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

            if (ApplicationRole == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string name = Request.Form["ApplicationRole.Name"];
            ApplicationRole.NormalizedName = name.ToUpper(CultureInfo.CurrentCulture);

            db.Attach(ApplicationRole).State = EntityState.Modified;

            try
            {
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationRoleExists(ApplicationRole.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Função editada com sucesso.");
        }

        private bool ApplicationRoleExists(string id)
        {
            return db.ApplicationRoles.Any(e => e.Id == id);
        }
    }
}
