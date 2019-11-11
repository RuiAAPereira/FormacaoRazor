using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName) { }

        public ApplicationRole(string roleName, string description,
            DateTime createdDate)
            : base(roleName)
        {
            base.Name = roleName;

            Description = description;
            CreatedDate = createdDate;
        }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
