using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using System.Globalization;
using FormacaoRazor.Models;

namespace FormacaoRazor.Pages.FormacoesColaboradorPages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public EditModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public FormacaoColaborador FormacaoColaborador { get; set; }
        public Formacao Formacoes { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        [BindProperty]
        public Guid FormacaoId2 { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FormacaoColaborador = await db.FormacoesColaboradores
                .Include(f => f.Colaborador)
                .Include(f => f.Formacao).FirstOrDefaultAsync(m => m.FormacaoColaboradorId == id).ConfigureAwait(false);

            if (FormacaoColaborador == null)
            {
                return NotFound();
            }
            ViewData["ColaboradorId"] = new SelectList(db.Colaboradores, "ColaboradorID", "Nome");
            ViewData["FormacaoId"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoCod");
            ViewData["FormacaoNome"] = new SelectList(db.Formacoes, "FormacaoId", "FormacaoNome");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Guid formacaoId = new Guid(Request.Form["FormacaoColaborador.FormacaoId"]);
            //Guid formandoId = new Guid(Request.Form["FormacaoColaborador.ColaboradorId"]);

            //List<FormacaoColaborador> jaTemFormacao = db.FormacoesColaboradores
            //    .Where(i => i.FormacaoId == formacaoId &&
            //    i.ColaboradorId == formandoId &&
            //    i.FormacaoColaboradorId != FormacaoColaborador.FormacaoColaboradorId).ToList();

            int validade = db.Formacoes.Where(i => i.FormacaoId == formacaoId).Select(v => v.FormacaoValidade).FirstOrDefault();

            DateTime dataAcao = DateTime.Parse(Request.Form["FormacaoData"], CultureInfo.GetCultureInfo("pt-PT"));

            FormacaoColaborador.FormacaoData = dataAcao;
            FormacaoColaborador.FormacaoCaducidade = dataAcao.AddYears(validade);

            //if (jaTemFormacao.Count >= 1)
            //{
            //    foreach (var item in jaTemFormacao)
            //    {
            //        HistoricoFormacaoColaborador = new HistoricoFormacaoColaborador
            //        {
            //            FormacaoId = item.FormacaoId,
            //            ColaboradorId = item.ColaboradorId,
            //            FormacaoData = item.FormacaoData,
            //            FormacaoCaducidade = item.FormacaoCaducidade,
            //            RefreshRequired = item.RefreshRequired
            //        };

            //        _ = db.HistoricoFormacoesColaboradores.Add(HistoricoFormacaoColaborador);
            //        _ = db.FormacoesColaboradores.Remove(item);
            //    }
            //}

            db.Attach(FormacaoColaborador).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormacaoColaboradorExists(FormacaoColaborador.FormacaoColaboradorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FormacaoColaboradorExists(Guid id)
        {
            return db.FormacoesColaboradores.Any(e => e.FormacaoColaboradorId == id);
        }
    }
}
