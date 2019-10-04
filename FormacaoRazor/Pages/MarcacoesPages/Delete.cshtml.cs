using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.MarcacoesPages
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
        public Marcacao Marcacao { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Marcacao = await db.Marcacoes
                .Include(m => m.Formacao)
                .Include(m => m.Formador)
                .Include(m => m.Sala)
                .Include(m => m.Uh).FirstOrDefaultAsync(m => m.MarcacaoID == id).ConfigureAwait(false);

            if (Marcacao == null)
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

            Marcacao = await db.Marcacoes.FindAsync(id).ConfigureAwait(true);

            if (Marcacao != null)
            {
                db.Marcacoes.Remove(Marcacao);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithDanger("OK!", "Marcação apagada com sucesso.");
        }
    }
}
