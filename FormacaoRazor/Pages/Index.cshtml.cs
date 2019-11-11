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
using System.IO;
using OfficeOpenXml;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace FormacaoRazor.Pages
{
    [DefaultBreadcrumb("Início")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IndexModel(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IList<Calendario> Calendarios { get; set; }
        public IList<Sala> Salas { get; set; }
        public IList<Marcacao> Marcacoes { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
        public DateTime CurrDate { get; set; }
        public Guid GuidUh { get; set; }
        public Guid GuidFormador { get; set; }

        public void OnGet(Guid uh)
        {
            DateTime DateNow = DateTime.UtcNow;

            OnGetData(DateNow, uh, null);
        }

        public void OnPostNext(DateTime currDate, Guid? uh)
        {
            DateTime DateNext = currDate.AddMonths(1);
            OnGetData(DateNext, uh, null);
        }

        public void OnPostPrev(DateTime currDate, Guid? uh)
        {
            DateTime DatePrev = currDate.AddMonths(-1);
            OnGetData(DatePrev, uh, null);
        }

        public void OnPostUh(DateTime currDate, Guid? uh)
        {
            OnGetData(currDate, uh, null);
        }

        public void OnPostFormador(DateTime currDate, Guid? uh)
        {
            GuidFormador = new Guid(Request.Form["GuidFormador"]);
            OnGetData(currDate, uh, GuidFormador);

        }

        public void OnGetData(DateTime currDate, Guid? uh, Guid? formador)
        {
            GuidUh = uh == Guid.Empty ? new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") : new Guid(Request.Form["GuidUh"]);

            ViewData["Datas"] = new SelectList(db.Calendarios, "Date", "Date");
            ViewData["Uhs"] = new SelectList(db.Uhs, "UhId", "IATA", GuidUh);
            ViewData["Formadores"] = new SelectList(db.Formadores.Where(i => i.UhId == GuidUh), "FormadorID", "FormadorNome");

            DateTime DateNow = DateTime.UtcNow;

            if (currDate == null)
            {
                FirstDayOfMonth = new DateTime(DateNow.Year, DateNow.Month, 1);
            }
            else if (currDate == new DateTime(2019, 1, 1))
            {
                FirstDayOfMonth = new DateTime(2019, 1, 1);
            }
            else
            {
                FirstDayOfMonth = new DateTime(currDate.Year, currDate.Month, 1);
            }

            var lastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

            Marcacoes = string.IsNullOrEmpty(formador.ToString())
                ? db.Marcacoes
                    .Where(i => i.UhId == GuidUh)
                    .Include(c => c.Formacao)
                    .Include(f => f.Formador)
                    .Include(c => c.Uh)
                    .ToList()
                : db.Marcacoes
                    .Where(i => i.UhId == GuidUh && i.FormadorID == formador)
                    .Include(c => c.Formacao)
                    .Include(f => f.Formador)
                    .Include(c => c.Uh)
                    .ToList();

            Calendarios = db.Calendarios.Where(d => d.Date >= FirstDayOfMonth && d.Date <= lastDayOfMonth).ToList();
            Salas = db.Salas.Where(i => i.UhId == GuidUh).OrderBy(n => n.SalaNome).ToList();
        }

        public void OnPostExport(DateTime currDate, Guid? uh)
        {

            //create a new ExcelPackage

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                DataTable table = new DataTable();

                foreach (Calendario dia in db.Calendarios.ToList())
                {
                    _ = table.Columns.Add();
                }
                foreach (Marcacao item in db.Marcacoes.ToList())
                {
                    _ = table.Rows.Add(item);
                }
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //add all the content from the DataTable, starting at cell A1
                worksheet.Cells["A1"].LoadFromDataTable(table, true);

                string projectRootPath = _hostingEnvironment.ContentRootPath;

                FileInfo fi = new FileInfo(projectRootPath + "/Temp/Text.xlsx");
                excelPackage.SaveAs(fi);

                table.Dispose();

                OnGetData(currDate, uh, null);
            }

        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }

            if (items == null) { throw new NullReferenceException(); }

            for (int i1 = 0; i1 < items.Count; i1++)
            {
                T item = items[i1];
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }

}
