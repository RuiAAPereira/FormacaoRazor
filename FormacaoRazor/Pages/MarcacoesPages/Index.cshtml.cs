using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Marcacoes;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.MarcacoesPages
{
    [Breadcrumb("Marcações")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Marcacao> Marcacao { get;set; }

        public async Task OnGetAsync()
        {
            Marcacao = await db.Marcacoes
                .Include(m => m.Formacao)
                .Include(m => m.Formador)
                .Include(m => m.Sala)
                .Include(m => m.Uh).ToListAsync().ConfigureAwait(false);
        }
    }
}
