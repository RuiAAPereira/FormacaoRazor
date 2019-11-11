using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.Roles
{
    //[Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<ApplicationRole> ApplicationRoleList { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationRoleList = await db.ApplicationRoles.ToListAsync().ConfigureAwait(false);
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationRole = await db.ApplicationRoles.FindAsync(id).ConfigureAwait(false);

            if (ApplicationRole != null)
            {
                db.ApplicationRoles.Remove(ApplicationRole);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Função apagada.");
        }

        public PartialViewResult OnGetAddModalPartial()
        {

            return new PartialViewResult
            {
                ViewName = "_AddModalPartial",
                ViewData = new ViewDataDictionary<ApplicationRole>(ViewData, new ApplicationRole { })
            };
        }

        public PartialViewResult OnPostAddModalPartial(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                string name = Request.Form["Name"];

                if (model != null)
                {
                    model.NormalizedName = name.ToUpper(CultureInfo.CurrentCulture);
                    _ = db.ApplicationRoles.Add(model);
                    _ = db.SaveChangesAsync().ConfigureAwait(true);

                    //return RedirectToPage("Index").WithSuccess("OK!", "Função adicionada com sucesso.");
                }
                else
                {
                    return new PartialViewResult
                    {
                        ViewName = "_AddModalPartial",
                        ViewData = new ViewDataDictionary<ApplicationRole>(ViewData, model)
                    };
                }
            }

            return new PartialViewResult
            {
                ViewName = "_AddModalPartial",
                ViewData = new ViewDataDictionary<ApplicationRole>(ViewData, model)
            };

        }

        public PartialViewResult OnGetEditModalPartial(string id)
        {
            //id = "6d2460ff-2428-4f9c-87be-b6e9617a10c3";
            ApplicationRole = db.ApplicationRoles.Find(id);

            return new PartialViewResult
            {
                ViewName = "_EditModalPartial",
                ViewData = new ViewDataDictionary<ApplicationRole>(ViewData, ApplicationRole)
            };
        }

    }
}
