using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormacaoRazor.Models
{
    [Table("FormacoesFormadores")]
    public class FormacoesFormador : IBaseEntity
    {
        [Key]
        public Guid FormacoesFormadorId { get; set; }
        //link com tabela Formações
        [Display(Name = "Formação:")]
        public Guid FormacaoId { get; set; }
        public Formacao Formacao { get; set; }
        //link com tabela Formadores
        [Display(Name = "Formador:")]
        public Guid FormadorId { get; set; }
        public Formador Formador { get; set; }

        [Display(Name = "Registo criado em:", ShortName = "Criado em:")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Registo criado por:", ShortName = "Criado por:")]
        public string CreatedBy { get; set; }
        [Display(Name = "Registo atualizado em:", ShortName = "Atualizado em:")]
        public DateTime? LastUpdatedAt { get; set; }
        [Display(Name = "Registo atualizado por:", ShortName = "Atualizado por:")]
        public string LastUpdatedBy { get; set; }
    }
}
