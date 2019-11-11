using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;
using Microsoft.AspNetCore.Mvc;
using FormacaoRazor.Extensions.Alerts;
using System;

namespace FormacaoRazor.Pages.FormacoesPages
{
    [Breadcrumb("Formações")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Formacao> FormacaoList { get; set; }
        [BindProperty]
        public Formacao Formacao { get; set; }
        public async Task OnGetAsync()
        {
            FormacaoList = await db.Formacoes.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Formacao = await db.Formacoes.FindAsync(new Guid(id)).ConfigureAwait(false);

            if (Formacao != null)
            {
                _ = db.Formacoes.Remove(Formacao);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Curso apagado.");
        }

    }
}
