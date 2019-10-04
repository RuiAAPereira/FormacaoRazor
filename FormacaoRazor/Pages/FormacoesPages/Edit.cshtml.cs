using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.FormacoesPages
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
        public Formacao Formacao { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Formacao = await db.Formacoes.FirstOrDefaultAsync(m => m.FormacaoId == id).ConfigureAwait(false);

            if (Formacao == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(Formacao).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormacaoExists(Formacao.FormacaoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("Index").WithInfo("OK!", "Formação alterada com sucesso.");
        }

        private bool FormacaoExists(Guid id)
        {
            return db.Formacoes.Any(e => e.FormacaoId == id);
        }
    }
}
