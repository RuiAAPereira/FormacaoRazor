using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Detalhes", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

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
    }
}
