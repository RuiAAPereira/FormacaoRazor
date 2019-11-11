using FormacaoRazor.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class UserDepartamento
    {
        [Key]
        public Guid UserDepartamentoId { get; set; }

        public Guid DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
