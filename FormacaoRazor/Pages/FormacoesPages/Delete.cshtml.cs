using System;
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
    [Breadcrumb("Apagar", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DeleteModel(ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Formacao = await db.Formacoes.FindAsync(id).ConfigureAwait(true);

            if (Formacao != null)
            {
                db.Formacoes.Remove(Formacao);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithDanger("OK!", "Formação apagada com sucesso.");
        }
    }
}
