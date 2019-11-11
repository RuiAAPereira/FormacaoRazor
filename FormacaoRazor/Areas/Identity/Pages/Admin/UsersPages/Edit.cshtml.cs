using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.UsersPages
{
    [Breadcrumb("Editar", FromPage = typeof(IndexModel))]
    public class EditModel : UserAssignmentsPageModel
    {
        private readonly ApplicationDbContext db;
        public ApplicationUserRole ApplicationUserRole { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        [Display(Name = "Função")]
        public Guid SelectedRole { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = db.ApplicationUsers
                .Include(r => r.ApplicationUserRoles).ThenInclude(r => r.Role)
                .Include(d => d.UserDepartamento).ThenInclude(d => d.Departamento)
                .Include(u => u.UserUhs).ThenInclude(u => u.Uh)
                .AsNoTracking()
                .FirstOrDefault(i => i.Id == id.ToString());

            string roleId = db.ApplicationUserRoles.Where(i => i.UserId == id.ToString()).Select(r => r.RoleId).FirstOrDefault();

            if (roleId != null)
            {
                SelectedRole = new Guid(roleId);
            }

            ViewData["RoleNames"] = new SelectList(db.ApplicationRoles.OrderBy(n => n.Name), "Id", "Name", SelectedRole);

            if (ApplicationUser == null)
            {
                return NotFound();
            }

            PopulateAssignedUhData(db, ApplicationUser);
            PopulateAssignedDepartamentoData(db, ApplicationUser);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id, string[] selectedUhs, string[] selectedDepartamentos)
        {
            if (SelectedRole == null) { throw new NullReferenceException(); }
            SelectedRole = new Guid(Request.Form["SelectedRole"]);

            ApplicationUser userToUpdate = db.ApplicationUsers
                .Include(i => i.UserUhs)
                .Include(d => d.UserDepartamento)
                .FirstOrDefault(i => i.Id == id.ToString());

            UpdateUserUhs(db, selectedUhs, userToUpdate);
            PopulateAssignedUhData(db, userToUpdate);

            UpdateUserDepartamentos(db, selectedDepartamentos, userToUpdate);
            PopulateAssignedDepartamentoData(db, userToUpdate);

            List<ApplicationUserRole> rolesToDelete = await db.ApplicationUserRoles
                .Where(i => i.UserId == id.ToString()).ToListAsync().ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            userToUpdate.Name = Request.Form["ApplicationUser.Name"];
            userToUpdate.UserName = Request.Form["ApplicationUser.UserName"];

            if (rolesToDelete.Any())
            {
                db.ApplicationUserRoles.RemoveRange(rolesToDelete);
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }

            var userRole = new ApplicationUserRole
            {
                UserId = id.ToString(),
                RoleId = SelectedRole.ToString()
            };

            _ = db.ApplicationUserRoles.Add(userRole);

            await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Index").WithInfo("OK!", "Utilizador alterado com sucesso.");
        }

    }
}
