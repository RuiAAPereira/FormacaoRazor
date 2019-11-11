using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using SmartBreadcrumbs.Attributes;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Extensions.Alerts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormacaoRazor.Pages.FormacoesColaboradorPages
{
    [Breadcrumb("Formações Colaboradores")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<FormacaoColaborador> FormacaoColaboradorList { get; set; }
        public FormacaoColaborador FormacaoColaborador { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        private List<Guid> ColaboradoresAtivos { get; set; }
        public Guid GuidUh { get; set; }
        public Guid GuidDepartamento { get; set; }
        [BindProperty]
        public Guid FormacaoId2 { get; set; }

        public void OnGet(Guid uh, Guid departamento)
        {
            OnGetData(uh, departamento);
        }

        public void OnGetData(Guid? uh, Guid? departamento)
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Guid> userUhs = db.UsersUhs
                .Where(i => i.User.Id == uId)
                .Select(u => u.UhId)
                .ToList();

            Guid firstUh = db.UsersUhs
                .Where(i => i.User.Id == uId)
                .Select(u => u.UhId)
                .FirstOrDefault();

            List<Guid> userDepartamentos = db.UsersDepartamentos
                .Where(i => i.User.Id == uId)
                .Select(d => d.DepartamentoId)
                .ToList();

            Guid firstDepartamento = db.UsersDepartamentos
                 .Where(i => i.User.Id == uId)
                .Select(d => d.DepartamentoId)
                .FirstOrDefault();

            ViewData["Uhs"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)),
                "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            GuidUh = uh == Guid.Empty ? firstUh : new Guid(Request.Form["GuidUh"]);

            ViewData["Departamentos"] = new SelectList(db.Departamentos
                .Where(i => userDepartamentos.Contains(i.DepartamentoId)),
                "DepartamentoId", "DepartamentoNome", null);

            GuidDepartamento = departamento == Guid.Empty ? firstDepartamento : new Guid(Request.Form["GuidDepartamento"]);
            ColaboradoresAtivos = db.Colaboradores.Where(a => a.Ativo == true).Select(i => i.ColaboradorID).ToList();

            DateTime caducidade = DateTime.UtcNow.AddMonths(3);

            FormacaoColaboradorList = db.FormacoesColaboradores
                .Include(f => f.Colaborador)
                .Include(f => f.Formacao)
                .Where(r => r.RefreshRequired == true && ColaboradoresAtivos.Contains(r.ColaboradorId))
                .Where(u => u.Colaborador.UhId == GuidUh)
                .Where(d => d.Colaborador.DepartamentoId == GuidDepartamento)
                .Where(d=>d.FormacaoCaducidade <= caducidade)
                .OrderBy(d => d.FormacaoCaducidade)
                .ToList();
        }

        public void OnPostSort(Guid? uh, Guid? departamento)
        {
            OnGetData(uh, departamento);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FormacaoColaborador = await db.FormacoesColaboradores.FindAsync(new Guid(id)).ConfigureAwait(false);

            if (FormacaoColaborador != null)
            {
                _ = db.FormacoesColaboradores.Remove(FormacaoColaborador);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Registo apagado.");
        }

        public async Task<IActionResult> OnPostArchive(string id)
        {
            FormacaoColaborador = db.FormacoesColaboradores
                .Where(i => i.FormacaoColaboradorId == new Guid(id))
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

            return RedirectToPage("Index").WithInfo("Ok", "Registo enviado para histórico");
        }
    }
}
