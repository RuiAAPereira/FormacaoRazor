using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.DepartamentoPages
{
    [Breadcrumb("Editar", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Departamento Departamento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departamento = await db.Departamentos
                .Include(d => d.Uh).FirstOrDefaultAsync(m => m.DepartamentoId == id).ConfigureAwait(true);

            if (Departamento == null)
            {
                return NotFound();
            }
           ViewData["UhId"] = new SelectList(db.Uhs, "UhId", "IATA");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(Departamento).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(Departamento.DepartamentoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("Index").WithInfo("OK!", "Departamento editado com sucesso.");
        }

        private bool DepartamentoExists(Guid id)
        {
            return db.Departamentos.Any(e => e.DepartamentoId == id);
        }
    }
}
