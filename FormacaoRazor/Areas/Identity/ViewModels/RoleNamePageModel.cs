using FormacaoRazor.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FormacaoRazor.Areas.Identity.ViewModels
{
    public class RoleNamePageModel : PageModel
    {
        public SelectList RoleNameSL { get; set; }

        public void PopulateRolesDropDownList(ApplicationDbContext db,
            object selectedDepartment = null)
        {
            if (db == null) { throw new NullReferenceException(); }

            var rolesQuery = from d in db.Roles
                             orderby d.Name // Sort by name.
                             select d;

            RoleNameSL = new SelectList(rolesQuery.AsNoTracking(),
                        "Name", "Name", selectedDepartment);
        }
    }
}
