using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Formandos;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Authorization;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.FormandosPages.ColaboradoresPages
{
    [Breadcrumb("Apagar", FromPage = typeof(IndexModel))]
    [Authorize(Roles = "Admin, Administrativo")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DeleteModel(ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Colaborador = await db.Colaboradores.FindAsync(id).ConfigureAwait(false);

            if (Colaborador != null)
            {
                db.Colaboradores.Remove(Colaborador);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("./Index");
        }
    }
}
