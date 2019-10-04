using FormacaoRazor.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace FormacaoRazor.Data
{
    public static class InitialUserData
    {
        public static async Task Initialize(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<ApplicationRole> roleManager)
        {
            if (context == null) { throw new NullReferenceException(); }
            if (roleManager == null) { throw new NullReferenceException(); }
            if (userManager == null) { throw new NullReferenceException(); }

            _ = context.Database.EnsureCreated();

            string role1 = "Administrador";
            string desc1 = "Administrador tem acesso a tudo";

            string role2 = "Formador";
            string desc2 = "Formador";

            string role3 = "Utilizador";
            string desc3 = "Utilizador apenas consulta";

            string password = "123456";

            if (await roleManager.FindByNameAsync(role1).ConfigureAwait(true) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now)).ConfigureAwait(true);
            }

            if (await roleManager.FindByNameAsync(role2).ConfigureAwait(true) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now)).ConfigureAwait(true);
            }

            if (await roleManager.FindByNameAsync(role3).ConfigureAwait(true) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role3, desc3, DateTime.Now)).ConfigureAwait(true);
            }

            if (await userManager.FindByNameAsync("rui.santos@portway.pt").ConfigureAwait(true) == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "rui.santos@portway.pt",
                    Email = "rui.santos@portway.pt",
                    Name = "Rui Pereira"
                };

                var result = await userManager.CreateAsync(user).ConfigureAwait(true);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, "rui050476").ConfigureAwait(true);
                    await userManager.AddToRoleAsync(user, role1).ConfigureAwait(true);
                }
                _ = user.Id;
            }

            if (await userManager.FindByNameAsync("formacao@portway.pt").ConfigureAwait(true) == null)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "formacao@portway.pt",
                    Email = "formacao@portway.pt",
                    Name = "Centro de Formação"
                };

                var result = await userManager.CreateAsync(user).ConfigureAwait(true);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password).ConfigureAwait(true);
                    userManager.AddToRoleAsync(user, role2).Wait();
                }

                _ = user.Id;
            }
        }
    }
}
