using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models;
using System.Security.Claims;

namespace FormacaoRazor.Pages.FormacoesColaboradorPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

        public Formacao Formacao { get; set; }
        public IList<FormacaoColaborador> FormacaoColaboradorList { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Guid> userUhs = db.UsersUhs
                .Where(i => i.User.Id == uId)
                .Select(u => u.UhId)
                .ToList();

            Formacao = await db.Formacoes.FirstOrDefaultAsync(m => m.FormacaoId == id).ConfigureAwait(false);

            FormacaoColaboradorList = await db.FormacoesColaboradores
                .Include(f => f.Formacao)
                .Include(c => c.Colaborador)
                .Where(u => userUhs.Contains(u.Colaborador.UhId))
                .Where(i => i.FormacaoId == id)
                .ToListAsync()
                .ConfigureAwait(false);

            if (Formacao == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
