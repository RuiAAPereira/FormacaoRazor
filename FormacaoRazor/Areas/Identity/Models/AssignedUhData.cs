using System;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class AssignedUhData
    {
        public Guid UhId { get; set; }
        public string Nome { get; set; }
        public bool Assigned { get; set; }
    }
}
