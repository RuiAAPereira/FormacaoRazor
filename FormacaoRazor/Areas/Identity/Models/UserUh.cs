using FormacaoRazor.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class UserUh
    {
        [Key]
        public Guid UserUhId { get; set; }

        public Guid UhId { get; set; }
        public Uh Uh { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
