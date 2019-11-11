using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;

namespace FormacaoRazor.Areas.Identity.Pages.Account.Manage
{
    [Breadcrumb("Password")]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password Atual")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A {0} deverá de ter entre {2} e {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar nova password")]
            [Compare("NewPassword", ErrorMessage = "As duas passwords não combinam.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com o ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(true);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com o ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword).ConfigureAwait(true);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user).ConfigureAwait(true);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "A sua password foi alterada com sucesso.";

            return RedirectToPage().WithSuccess("OK!", "A sua password foi alterada com sucesso.");
        }
    }
}
