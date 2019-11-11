using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Models.Formandos;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Authorization;
using FormacaoRazor.Data;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Extensions.Helpers;

namespace FormacaoRazor.Pages.FormandosPages.ColaboradoresPages
{
    [Breadcrumb("Criar", FromPage = typeof(IndexModel))]
    [Authorize(Roles = "Admin, Administrativo")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public CreateModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult OnGet()
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Guid> userUhs = db.UsersUhs
                .Where(i => i.User.Id == uId)
                .Select(u => u.UhId)
                .ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)),
                "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            List<Guid> userDepartamentos = db.UsersDepartamentos
                .Where(i => i.User.Id == uId)
                .Select(d => d.DepartamentoId)
                .ToList();

            ViewData["DepartamentoId"] = new SelectList(db.Departamentos
                .Where(i => userDepartamentos.Contains(i.DepartamentoId)),
                "DepartamentoId", "DepartamentoNome", null);

            ViewData["FuncaoId"] = new SelectList(db.Funcoes, "FuncaoId", "FuncaoNome");

            return Page();
        }

        [BindProperty]
        public Colaborador Colaborador { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _ = db.Colaboradores.Add(Colaborador);
            try
            {
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateException ex)
            {
                if (ExceptionHelper.IsUniqueConstraintViolation(ex))
                {
                    ModelState.AddModelError(string.Empty, $"O Nome '{Colaborador.Nome}' já está atribuido");
                    return OnGet();
                }
            }

            return RedirectToPage("./Index").WithSuccess("OK!", "Colaborador adicionado com sucesso.");
        }
    }
}