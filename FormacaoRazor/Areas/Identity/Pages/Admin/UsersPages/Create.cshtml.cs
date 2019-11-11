using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Data;
using System.Linq;
using System;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.EntityFrameworkCore;

namespace FormacaoRazor.Areas.Identity.Pages.Admin.UsersPages
{
    public class CreateModel : UserAssignmentsPageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext db;

        public CreateModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            db = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public Uri ReturnUrl { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
        public class InputModel
        {
            [Required]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Função")]
            public Guid SelectedRole { get; set; }

        }

        public void OnGet(Uri returnUrl = null)
        {
            ViewData["RoleNames"] = new SelectList(
                db.ApplicationRoles.OrderBy(n => n.Name),
                "Id", "Name", "b5110b08-6c8a-4866-b7e8-d6dffd96a7de");

            PopulateAllUhData(db);
            PopulateAllDepartamentoData(db);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedUhs, string[] selectedDepartamentos)
        {
            if (ModelState.IsValid)
            {
                if (UserExists(Input.Email))
                {
                    ModelState.AddModelError(string.Empty, "Erro: Email já atribuido");

                    PopulateAllUhData(db);
                    PopulateAllDepartamentoData(db);

                    return Page();
                }
                ApplicationUser user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Nome
                };

                IdentityResult result = await _userManager.CreateAsync(user, Input.Password).ConfigureAwait(true);

                if (result.Succeeded)
                {
                    ApplicationUser userToUpdate = db.ApplicationUsers
                        .Include(i => i.UserUhs)
                        .Include(d => d.UserDepartamento)
                        .FirstOrDefault(i => i.Id == user.Id);

                    UpdateUserUhs(db, selectedUhs, userToUpdate);
                    UpdateUserDepartamentos(db, selectedDepartamentos, userToUpdate);

                    ApplicationUserRole userRole = new ApplicationUserRole
                    {
                        UserId = user.Id,
                        RoleId = Input.SelectedRole.ToString()
                    };

                    _ = db.ApplicationUserRoles.Add(userRole);
                    _ = await db.SaveChangesAsync().ConfigureAwait(true);

                    return RedirectToPage("Index").WithSuccess("OK!", "Utilizador Criado com sucesso.");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            PopulateAllUhData(db);
            PopulateAllDepartamentoData(db);
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool UserExists(string email)
        {
            return db.ApplicationUsers.Any(e => e.Email == email);
        }

    }
}