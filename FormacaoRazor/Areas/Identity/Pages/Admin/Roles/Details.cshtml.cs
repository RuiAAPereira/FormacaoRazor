using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.Roles
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

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
    }
}
