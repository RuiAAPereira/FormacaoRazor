using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.DepartamentoPages
{
    [Breadcrumb("Departamentos")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Departamento> Departamentos { get;set; }

        public async Task OnGetAsync()
        {
            Departamentos = await db.Departamentos
                .Include(d => d.Uh).ToListAsync().ConfigureAwait(true);
        }

    }
}
