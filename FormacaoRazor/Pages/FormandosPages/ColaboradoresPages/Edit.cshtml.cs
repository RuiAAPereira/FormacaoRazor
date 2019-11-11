using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Formandos;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Authorization;
using FormacaoRazor.Data;
using System.Security.Claims;
using System.Collections.Generic;

namespace FormacaoRazor.Pages.FormandosPages.ColaboradoresPages
{
    [Breadcrumb("Editar", FromPage = typeof(IndexModel))]
    [Authorize(Roles = "Admin, Administrativo")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Colaborador Colaborador { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Colaborador = await db.Colaboradores
                .Include(c => c.Departamento)
                .Include(c => c.Funcao)
                .Include(c => c.Uh).FirstOrDefaultAsync(m => m.ColaboradorID == id).ConfigureAwait(false);

            if (Colaborador == null)
            {
                return NotFound();
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(Colaborador).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColaboradorExists(Colaborador.ColaboradorID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ColaboradorExists(Guid id)
        {
            return db.Colaboradores.Any(e => e.ColaboradorID == id);
        }
    }
}
