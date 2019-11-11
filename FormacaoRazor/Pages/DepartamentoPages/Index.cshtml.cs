using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using SmartBreadcrumbs.Attributes;
using FormacaoRazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.DepartamentoPages
{
    [Breadcrumb("Departamentos")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public IndexModel(ApplicationDbContext context)
        {
            db = context;
        }

        public IList<Departamento> Departamentos { get;set; }
        [BindProperty]
        public Departamento Departamento { get; set; }
        public async Task OnGetAsync()
        {
            Departamentos = await db.Departamentos
                .ToListAsync().ConfigureAwait(true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departamento = await db.Departamentos.FindAsync(new Guid(id)).ConfigureAwait(false);

            if (Departamento != null)
            {
                _ = db.Departamentos.Remove(Departamento);
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            return RedirectToPage("Index").WithSuccess("OK!", "Função apagada.");
        }

        public PartialViewResult OnGetAddModalPartial()
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Guid> userUhs = db.UsersUhs.Where(i => i.User.Id == uId).Select(u => u.UhId).ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)), "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            return new PartialViewResult
            {
                ViewName = "_AddModalPartial",
                ViewData = new ViewDataDictionary<Departamento>(ViewData, new Departamento { })
            };
        }

        public PartialViewResult OnPostAddModalPartial(Departamento model)
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Guid> userUhs = db.UsersUhs.Where(i => i.User.Id == uId).Select(u => u.UhId).ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)), "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            if (ModelState.IsValid)
            {
                if (model != null)
                {

                    _ = db.Departamentos.Add(model);
                    _ = db.SaveChangesAsync().ConfigureAwait(true);
                }
            }

            return new PartialViewResult
            {
                ViewName = "_AddModalPartial",
                ViewData = new ViewDataDictionary<Departamento>(ViewData, model)
            };

        }

        public PartialViewResult OnGetEditModalPartial(string id)
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Guid> userUhs = db.UsersUhs.Where(i => i.User.Id == uId).Select(u => u.UhId).ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)), "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            Departamento = db.Departamentos.Find(new Guid(id));

            return new PartialViewResult
            {
                ViewName = "_EditModalPartial",
                ViewData = new ViewDataDictionary<Departamento>(ViewData, Departamento)
            };
        }

        public PartialViewResult OnPostEditModalPartial(Departamento model)
        {
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Guid> userUhs = db.UsersUhs.Where(i => i.User.Id == uId).Select(u => u.UhId).ToList();

            ViewData["UhId"] = new SelectList(db.Uhs.Where(i => userUhs.Contains(i.UhId)), "UhId", "IATA", "04fde6b2-fd97-4856-9640-b9e1abc73140");

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    _ = db.Attach(model).State = EntityState.Modified;
                    _ = db.SaveChangesAsync().ConfigureAwait(true);
                }
            }

            return new PartialViewResult
            {
                ViewName = "_EditModalPartial",
                ViewData = new ViewDataDictionary<Departamento>(ViewData, model)
            };

        }

    }
}
