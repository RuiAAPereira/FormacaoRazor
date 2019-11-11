using FormacaoRazor.Models.Formandos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormacaoRazor.Models.Formacoes
{
    [Table("HistoricoFormacoesColaboradores")]
    public partial class HistoricoFormacaoColaborador : IBaseEntity
    {
        public Guid HistoricoFormacaoColaboradorId { get; set; }
        //link com tabela Formações
        [Display(Name = "Formação:")]
        public Guid FormacaoId { get; set; }
        public Formacao Formacao { get; set; }
        //link com tabela Colaborador\
        [Display(Name = "Formando:")]
        public Guid ColaboradorId { get; set; }
        public Colaborador Colaborador { get; set; }
        [Display(Name = "Data:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FormacaoData { get; set; }
        [Display(Name = "Caducidade:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FormacaoCaducidade { get; set; }

        public bool RefreshRequired { get; set; } = true;

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
