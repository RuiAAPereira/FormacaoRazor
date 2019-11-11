using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Formandos;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using System.Collections.Generic;
using System.Linq;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.FormandosPages.ColaboradoresPages
{
    [Breadcrumb("Detalhe", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DetailsModel(ApplicationDbContext context)
        {
            db = context;
        }

        public Colaborador Colaborador { get; set; }
        public FormacaoColaborador FormacaoColaborador { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        public IList<FormacaoColaborador> FormacaoColaboradorList { get; set; }
        public IList<HistoricoFormacaoColaborador> HistoricoFormacaoColaboradorList { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Colaborador = await db.Colaboradores
                .Include(c => c.Departamento)
                .Include(c => c.Funcao)
                .Include(c => c.Uh).FirstOrDefaultAsync(m => m.ColaboradorID == id).ConfigureAwait(false);

            FormacaoColaboradorList = await db.FormacoesColaboradores
                .Include(f => f.Formacao)
                .Where(i => i.ColaboradorId == id)
                .OrderBy(d => d.FormacaoCaducidade)
                .ToListAsync()
                .ConfigureAwait(false);

            HistoricoFormacaoColaboradorList = await db.HistoricoFormacoesColaboradores
                .Include(f => f.Formacao)
                .Where(i => i.ColaboradorId == id)
                .OrderBy(d => d.FormacaoCaducidade)
                .ToListAsync()
                .ConfigureAwait(false);

            if (Colaborador == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteFormacaoAsync(Guid? id, string idFormacao)
        {
            if (idFormacao == null)
            {
                return NotFound();
            }

            FormacaoColaborador = await db.FormacoesColaboradores.FindAsync(new Guid(idFormacao)).ConfigureAwait(false);

            if (FormacaoColaborador != null)
            {
                _ = db.FormacoesColaboradores.Remove(FormacaoColaborador);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Details", new { id }).WithSuccess("OK!", "Registo apagado.");
        }

        public async Task<IActionResult> OnPostDeleteHistoryAsync(Guid? id, string idHistory)
        {
            if (idHistory == null)
            {
                return NotFound();
            }

            HistoricoFormacaoColaborador = await db.HistoricoFormacoesColaboradores.FindAsync(new Guid(idHistory)).ConfigureAwait(false);

            if (HistoricoFormacaoColaborador != null)
            {
                _ = db.HistoricoFormacoesColaboradores.Remove(HistoricoFormacaoColaborador);
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Details", new { id }).WithSuccess("OK!", "Registo apagado.");
        }

        public async Task<IActionResult> OnPostArchive(Guid? id, string idFormacao)
        {
            FormacaoColaborador = db.FormacoesColaboradores
                .Where(i => i.FormacaoColaboradorId == new Guid(idFormacao))
                .FirstOrDefault();

            HistoricoFormacaoColaborador = new HistoricoFormacaoColaborador
            {
                HistoricoFormacaoColaboradorId = new Guid(),
                FormacaoId = FormacaoColaborador.FormacaoId,
                ColaboradorId = FormacaoColaborador.ColaboradorId,
                FormacaoData = FormacaoColaborador.FormacaoData,
                FormacaoCaducidade = FormacaoColaborador.FormacaoCaducidade,
                RefreshRequired = FormacaoColaborador.RefreshRequired
            };

            _ = db.HistoricoFormacoesColaboradores.Add(HistoricoFormacaoColaborador);
            _ = db.FormacoesColaboradores.Remove(FormacaoColaborador);

            _ = await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Details", new { id }).WithSuccess("OK!", "Registo enviado para histórico.");
        }

        public async Task<IActionResult> OnPostActivate(Guid? id, string idHistory)
        {
            HistoricoFormacaoColaborador = db.HistoricoFormacoesColaboradores
                .Where(i => i.HistoricoFormacaoColaboradorId == new Guid(idHistory))
                .FirstOrDefault();


            FormacaoColaborador = new FormacaoColaborador
            {
                FormacaoColaboradorId = new Guid(),
                FormacaoId = HistoricoFormacaoColaborador.FormacaoId,
                ColaboradorId = HistoricoFormacaoColaborador.ColaboradorId,
                FormacaoData = HistoricoFormacaoColaborador.FormacaoData,
                FormacaoCaducidade = HistoricoFormacaoColaborador.FormacaoCaducidade,
                RefreshRequired = HistoricoFormacaoColaborador.RefreshRequired
            };

            _ = db.HistoricoFormacoesColaboradores.Remove(HistoricoFormacaoColaborador);
            _ = db.FormacoesColaboradores.Add(FormacaoColaborador);

            _ = await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("Details", new { id }).WithSuccess("OK!", "Registo recuperado.");
        }

    }
}
