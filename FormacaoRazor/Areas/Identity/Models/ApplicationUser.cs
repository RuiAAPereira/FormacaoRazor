using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FormacaoRazor.Areas.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        [PersonalData]
        public string Name { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; }
    }
}
