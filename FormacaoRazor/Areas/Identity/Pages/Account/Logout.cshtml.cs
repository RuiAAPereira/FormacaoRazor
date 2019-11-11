using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FormacaoRazor.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormacaoRazor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public static void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(Uri returnUrl = null)
        {
            await _signInManager.SignOutAsync().ConfigureAwait(true);

            return returnUrl != null ? LocalRedirect(returnUrl.ToString()) : (IActionResult)RedirectToPage();
        }
    }
}