using System;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class AssignedDepartamentoData
    {
        public Guid DepartamentoId { get; set; }
        public string Nome { get; set; }
        public bool Assigned { get; set; }
    }
}
