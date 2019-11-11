using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class UserAppSettings
    {
        [Key]
        public Guid UserAppSettingId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserAppSettingName { get; set; }
        public string UserAppSettingValue { get; set; }
    }
}
