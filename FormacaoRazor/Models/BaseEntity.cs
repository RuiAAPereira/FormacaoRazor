using System;

namespace FormacaoRazor.Models
{
    public interface IBaseEntity
    {
        DateTime? CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastUpdatedAt { get; set; }
        string LastUpdatedBy { get; set; }
    }
}
