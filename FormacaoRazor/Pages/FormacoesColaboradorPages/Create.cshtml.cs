using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FormacaoRazor.Data;
using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models;
using System.Globalization;
using FormacaoRazor.Extensions.Alerts;

namespace FormacaoRazor.Pages.FormacoesColaboradorPages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public CreateModel(ApplicationDbContext context)
        {
            db = context;
        }

        public FormacaoColaborador FormacaoColaborador { get; set; }
        public Formacao Formacoes { get; set; }
        public HistoricoFormacaoColaborador HistoricoFormacaoColaborador { get; set; }
        [BindProperty]
        public Guid FormacaoId2 { get; set; }

        public IActionResult OnGet(string cId, string fId)
        {
            if (fId == null)
            {
                ViewData["FormacaoCod"] = new SelectList(db.Formacoes.OrderBy(n => n.FormacaoNome), "FormacaoId", "FormacaoCod");
                ViewData["FormacaoNome"] = new SelectList(db.Formacoes.OrderBy(n => n.FormacaoNome), "FormacaoId", "FormacaoNome");
            }
            else
            {
                ViewData["FormacaoCod"] = new SelectList(db.Formacoes.Where(i => i.FormacaoId == new Guid(fId)), "FormacaoId", "FormacaoCod");
                ViewData["FormacaoNome"] = new SelectList(db.Formacoes.Where(i => i.FormacaoId == new Guid(fId)), "FormacaoId", "FormacaoNome");
            }

            if (cId == null)
            {
                ViewData["ColaboradorId"] = new SelectList(db.Colaboradores.OrderBy(n => n.Nome), "ColaboradorID", "Nome");
            }
            else
            {
                ViewData["ColaboradorId"] = new SelectList(db.Colaboradores.Where(i => i.ColaboradorID == new Guid(cId)), "ColaboradorID", "Nome");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Guid formacaoId = new Guid(Request.Form["FormacaoCod"]);
            Guid colaboradorId = new Guid(Request.Form["ColaboradorId"]);

            List<FormacaoColaborador> jaTemFormacao = db.FormacoesColaboradores
                .Where(i => i.FormacaoId == formacaoId &&
                i.ColaboradorId == colaboradorId).ToList();

            if (jaTemFormacao.Count >= 1)
            {
                foreach (var item in jaTemFormacao)
                {
                    HistoricoFormacaoColaborador = new HistoricoFormacaoColaborador
                    {
                        FormacaoId = item.FormacaoId,
                        ColaboradorId = item.ColaboradorId,
                        FormacaoData = item.FormacaoData,
                        FormacaoCaducidade = item.FormacaoCaducidade,
                        RefreshRequired = item.RefreshRequired
                    };

                    _ = db.HistoricoFormacoesColaboradores.Add(HistoricoFormacaoColaborador);
                    _ = db.FormacoesColaboradores.Remove(item);
                }
            }

            _ = await db.SaveChangesAsync().ConfigureAwait(true);

            int validade = db.Formacoes.Where(i => i.FormacaoId == formacaoId).Select(v => v.FormacaoValidade).FirstOrDefault();

            DateTime dataAcao = DateTime.Parse(Request.Form["FormacaoData"], CultureInfo.GetCultureInfo("pt-PT"));

            FormacaoColaborador = new FormacaoColaborador
            {
                FormacaoId = formacaoId,
                ColaboradorId = colaboradorId,
                FormacaoData = dataAcao,
                FormacaoCaducidade = dataAcao.AddYears(validade)
            };

            _ = db.FormacoesColaboradores.Add(FormacaoColaborador);

            _ = await db.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToPage("./Index").WithSuccess("Ok", "Registo criado com sucesso!");
        }

    }
}