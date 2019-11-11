using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace FormacaoRazor.Pages.FormacoesColaboradorPages
{
    [Breadcrumb("planeamento")]
    public class PlaneamentoModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public PlaneamentoModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<FormacaoColaborador> FormacaoColaboradorList { get; set; }
        public FormacaoColaborador FormacaoColaborador { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        private List<Guid> ColaboradoresAtivos { get; set; }
        public Guid GuidUh { get; set; }
        public Guid GuidDepartamento { get; set; }
        public string StringDatas { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public void OnGet(Guid uh, Guid departamento, string datas)
        {
            OnGetData(uh, departamento, datas);
        }

        public void OnGetData(Guid? uh, Guid? departamento, string datas)
        {
            if (string.IsNullOrEmpty(datas))
            {
                int year = DateTime.Now.Year;
                DataInicio = new DateTime(year, 1, 1);
                DataFim = new DateTime(year, 12, 31);
            }
            else
            {
                string inicio = datas.Split(" - ")[0];
                string fim = datas.Split(" - ")[1];
                DataInicio = DateTime.Parse(inicio, CultureInfo.GetCultureInfo("pt-PT"));
                DataFim = DateTime.Parse(fim, CultureInfo.GetCultureInfo("pt-PT"));
            }

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

            FormacaoColaboradorList = db.FormacoesColaboradores
                .Include(f => f.Colaborador)
                .Include(f => f.Formacao)
                .Where(r => r.RefreshRequired == true && ColaboradoresAtivos.Contains(r.ColaboradorId))
                .Where(u => u.Colaborador.UhId == GuidUh)
                .Where(d => d.Colaborador.DepartamentoId == GuidDepartamento)
                .Where(d => d.FormacaoCaducidade >= DataInicio && d.FormacaoCaducidade <= DataFim)
                .OrderBy(d => d.FormacaoCaducidade)
                .ToList();
        }

        public void OnPostSort(Guid? uh, Guid? departamento, string datas)
        {
            OnGetData(uh, departamento, datas);
        }

        public void OnPostDatas(Guid? uh, Guid? departamento, string datas)
        {
            OnGetData(uh, departamento, datas);
        }

    }
}
