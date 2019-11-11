using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormacaoRazor.ViewComponents
{
    public class CaducidadesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly DateTime DataTresMeses = DateTime.UtcNow.Date.AddMonths(3);

        public CaducidadesViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listaCaducidades = await GetItemsAsync().ConfigureAwait(false);

            return View("ViewCaducidades", listaCaducidades);
        }

        private Task<List<FormacaoColaborador>> GetItemsAsync()
        {
            return db.FormacoesColaboradores
                .Include(f => f.Formacao)
                .Include(c => c.Colaborador).Where(a => a.Colaborador.Ativo == true)
                .Where(d => d.FormacaoCaducidade <= DataTresMeses).ToListAsync();
        }
    }
}
