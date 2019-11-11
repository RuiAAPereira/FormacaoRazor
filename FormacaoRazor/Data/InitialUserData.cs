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

            string role1 = "Admin";
            string desc1 = "Administrador tem acesso a tudo";

            string role2 = "Administrativo";
            string desc2 = "Permissões de criar/apagar etc...";

            string role3 = "Formador";
            string desc3 = "Formador";

            string role4 = "Utilizador";
            string desc4 = "Utilizador apenas consulta";

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
            if (await roleManager.FindByNameAsync(role3).ConfigureAwait(true) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role4, desc4, DateTime.Now)).ConfigureAwait(true);
            }

            if (await userManager.FindByNameAsync("rui.santos@portway.pt").ConfigureAwait(true) == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Id = new Guid("52692ac2-a069-4f97-a2fb-c801cec9d2ff").ToString(),
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
                    userManager.AddToRoleAsync(user, role3).Wait();
                }

                _ = user.Id;
            }
        }
    }
}
