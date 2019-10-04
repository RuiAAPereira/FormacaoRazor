using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;

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

        public IList<Formacao> Formacao { get;set; }
        public async Task OnGetAsync()
        {
            Formacao = await db.Formacoes.ToListAsync().ConfigureAwait(false);
        }
    }
}
