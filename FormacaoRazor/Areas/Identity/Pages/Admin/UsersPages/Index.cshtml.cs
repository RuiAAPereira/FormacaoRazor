using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.UsersPages
{
    [Breadcrumb("Utilizadores")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;
        private readonly Guid superAdminGuid = new Guid("52692ac2-a069-4f97-a2fb-c801cec9d2ff");

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<ApplicationUser> ApplicationUsersList { get; set; }
        public IList<ApplicationRole> ApplicationRoles { get; set; }
        public IList<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public IList<UserUh> UserUhs { get; set; }
        public IList<UserDepartamento> UserDepartamentos { get; set; }
        public IList<Uh> Uhs { get; set; }
        public IList<Departamento> Departamentos { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationUsersList = await db.ApplicationUsers
                .Include(r => r.ApplicationUserRoles)
                .ToListAsync().ConfigureAwait(false);

            ApplicationRoles = await db.ApplicationRoles
                .ToListAsync().ConfigureAwait(false);

            ApplicationUserRoles = await db.ApplicationUserRoles
                .ToListAsync().ConfigureAwait(false);

            UserUhs = await db.UsersUhs
                .ToListAsync().ConfigureAwait(false);

            Uhs = await db.Uhs.ToListAsync().ConfigureAwait(false);

            UserDepartamentos = await db.UsersDepartamentos
                .ToListAsync().ConfigureAwait(false);

            Departamentos = await db.Departamentos.ToListAsync().ConfigureAwait(false);

        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnPostDeleteAsync(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id == superAdminGuid)
            {
                return RedirectToPage("Index").WithWarning("Aviso!", "Não pode apagar esse utilizador.");
            }

            ApplicationUser = await db.ApplicationUsers.FindAsync(id.ToString()).ConfigureAwait(true);

            if (ApplicationUser != null)
            {
                ApplicationUserRoles = await db.ApplicationUserRoles
                    .Where(u => u.UserId == ApplicationUser.Id)
                    .ToListAsync().ConfigureAwait(false);

                UserUhs = await db.UsersUhs
                    .Where(u => u.User.Id == ApplicationUser.Id)
                    .ToListAsync().ConfigureAwait(false);

                UserDepartamentos = await db.UsersDepartamentos
                    .Where(u => u.User.Id == ApplicationUser.Id)
                    .ToListAsync().ConfigureAwait(false);

                db.ApplicationUserRoles.RemoveRange(ApplicationUserRoles);
                db.UsersUhs.RemoveRange(UserUhs);
                db.UsersDepartamentos.RemoveRange(UserDepartamentos);

                _ = db.ApplicationUsers.Remove(ApplicationUser);
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Utilizador apagado com sucesso.");
        }
    }
}
