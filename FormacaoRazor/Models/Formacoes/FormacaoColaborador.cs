using FormacaoRazor.Models.Formandos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormacaoRazor.Models.Formacoes
{
    [Table("FormacoesColaboradores")]
    public partial class FormacaoColaborador : IBaseEntity
    {
        public Guid FormacaoColaboradorId { get; set; }
        //link com tabela Formações
        [Display(Name = "Formação:")]
        public Guid FormacaoId { get; set; }
        public Formacao Formacao { get; set; }
        //link com tabela Colaborador
        [Display(Name = "Formando:")]
        public Guid ColaboradorId { get; set; }
        public Colaborador Colaborador { get; set; }
        [Display(Name = "Data:")]
        public DateTime FormacaoData { get; set; }
        [Display(Name = "Caducidade:")]
        public DateTime FormacaoCaducidade { get; set; }

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
