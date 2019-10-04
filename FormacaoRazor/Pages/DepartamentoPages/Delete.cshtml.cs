using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.DepartamentoPages
{
    [Breadcrumb("Apagar", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DeleteModel(ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departamento = await db.Departamentos.FindAsync(id).ConfigureAwait(true);

            if (Departamento != null)
            {
                db.Departamentos.Remove(Departamento);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithDanger("OK!", "Departamento apagado com sucesso.");
        }
    }
}
