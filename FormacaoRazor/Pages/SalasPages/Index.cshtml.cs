using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Common;

namespace FormacaoRazor.Pages.SalasPages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Sala> Sala { get;set; }

        public async Task OnGetAsync()
        {
            Sala = await _context.Salas
                .Include(s => s.Uh).ToListAsync().ConfigureAwait(false);
        }
    }
}
