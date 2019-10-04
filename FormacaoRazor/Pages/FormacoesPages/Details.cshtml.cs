using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.FormacoesPages
{
    [Breadcrumb("Detalhes", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

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
    }
}
