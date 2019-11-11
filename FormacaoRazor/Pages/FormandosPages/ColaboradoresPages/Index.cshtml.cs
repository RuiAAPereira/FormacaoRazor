using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Data;
using FormacaoRazor.Extensions.Alerts;
using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models.Formandos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FormacaoRazor.Pages.FormandosPages.ColaboradoresPages
{
    [Breadcrumb("Colaboradores")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Colaborador> ColaboradorList { get; set; }
        private IList<FormacaoColaborador> FormacaoColaboradoresList { get; set; }
        private IList<HistoricoFormacaoColaborador> HistoricoFormacaoColaboradoresList { get; set; }
        [BindProperty]
        public Colaborador Colaborador { get; set; }
        public FormacaoColaborador FormacaoColaborador { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        public Guid GuidUh { get; set; }
        public Guid GuidDepartamento { get; set; }

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

            ColaboradorList = db.Colaboradores
                .Where(u => u.UhId == GuidUh)
                .Where(d => d.DepartamentoId == GuidDepartamento)
                .Include(c => c.Departamento)
                .Include(c => c.Funcao)
                .Include(c => c.Uh).ToList();
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

            Colaborador = await db.Colaboradores.FindAsync(new Guid(id)).ConfigureAwait(false);
            FormacaoColaboradoresList = await db.FormacoesColaboradores.Where(ci => ci.ColaboradorId == Colaborador.ColaboradorID).ToListAsync().ConfigureAwait(false);
            HistoricoFormacaoColaboradoresList = await db.HistoricoFormacoesColaboradores.Where(ci => ci.ColaboradorId == Colaborador.ColaboradorID).ToListAsync().ConfigureAwait(false);

            if (Colaborador != null)
            {
                db.HistoricoFormacoesColaboradores.RemoveRange(HistoricoFormacaoColaboradoresList);
                db.FormacoesColaboradores.RemoveRange(FormacaoColaboradoresList);
                _ = db.Colaboradores.Remove(Colaborador);
                _ = await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Colaborador apagado.");
        }

    }
}
