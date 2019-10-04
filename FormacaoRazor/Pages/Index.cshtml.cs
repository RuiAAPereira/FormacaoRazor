using System;
using System.Collections.Generic;
using System.Linq;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Marcacoes;
using FormacaoRazor.Models.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormacaoRazor.Pages
{
    [DefaultBreadcrumb("Início")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Calendario> Calendarios { get; set; }
        public IList<Sala> Salas { get; set; }
        public IList<Marcacao> Marcacoes { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
        public Guid GuidUh { get; set; }
        public Guid GuidFormador { get; set; }
        public void OnGet(int mes, int ano, Guid uh)
        {
            OnPostUh( mes, ano, uh);
        }

        public void OnPostUh(int mes, int ano, Guid? uh)
        {
            GuidUh = uh == Guid.Empty ? new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") : new Guid(Request.Form["GuidUh"]);

            ViewData["Datas"] = new SelectList(db.Calendarios, "Date", "Date");
            ViewData["Uhs"] = new SelectList(db.Uhs, "UhId", "IATA", GuidUh);
            ViewData["Formadores"] = new SelectList(db.Formadores.Where(i => i.UhId == GuidUh), "FormadorID", "FormadorNome");

            DateTime DateNow = DateTime.UtcNow;

            if (mes <= 0 || ano <= 0)
            {
                FirstDayOfMonth = new DateTime(DateNow.Year, DateNow.Month, 1);
            }
            else if (ano <= 2018)
            {
                FirstDayOfMonth = new DateTime(2019, mes, 1);
            }
            else
            {
                FirstDayOfMonth = new DateTime(ano, mes, 1);
            }

            var lastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

            Marcacoes = db.Marcacoes
                .Where(i => i.UhId == GuidUh)
                .Include(c => c.Formacao)
                .Include(f => f.Formador)
                .Include(c => c.Uh)
                .ToList();
            Calendarios = db.Calendarios.Where(d => d.Date >= FirstDayOfMonth && d.Date <= lastDayOfMonth).ToList();
            Salas = db.Salas.Where(i => i.UhId == GuidUh).OrderBy(n => n.SalaNome).ToList();
        }

        public void OnPostFormador(int mes, int ano, Guid? uh)
        {
            GuidUh = uh == Guid.Empty ? new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") : new Guid(Request.Form["GuidUh"]);

            GuidFormador = new Guid(Request.Form["GuidFormador"]);

            ViewData["Datas"] = new SelectList(db.Calendarios, "Date", "Date");
            ViewData["Uhs"] = new SelectList(db.Uhs, "UhId", "IATA", GuidUh);
            ViewData["Formadores"] = new SelectList(db.Formadores.Where(i => i.UhId == GuidUh), "FormadorID", "FormadorNome");

            DateTime DateNow = DateTime.UtcNow;

            if (mes <= 0 || ano <= 0)
            {
                FirstDayOfMonth = new DateTime(DateNow.Year, DateNow.Month, 1);
            }
            else if (ano <= 2018)
            {
                FirstDayOfMonth = new DateTime(2019, mes, 1);
            }
            else
            {
                FirstDayOfMonth = new DateTime(ano, mes, 1);
            }

            var lastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

            Marcacoes = db.Marcacoes
                .Where(i => i.UhId == GuidUh && i.FormadorID == GuidFormador)
                .Include(c => c.Formacao)
                .Include(f => f.Formador)
                .Include(c => c.Uh)
                .ToList();
            Calendarios = db.Calendarios.Where(d => d.Date >= FirstDayOfMonth && d.Date <= lastDayOfMonth).ToList();
            Salas = db.Salas.Where(i => i.UhId == GuidUh).OrderBy(n => n.SalaNome).ToList();
        }
    }
}
