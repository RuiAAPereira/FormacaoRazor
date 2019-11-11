using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;

namespace FormacaoRazor.Pages.DepartamentoPages
{
    [Breadcrumb("Detalhes", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

        public Departamento Departamento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departamento = await db.Departamentos
                .FirstOrDefaultAsync(m => m.DepartamentoId == id).ConfigureAwait(true);

            if (Departamento == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
