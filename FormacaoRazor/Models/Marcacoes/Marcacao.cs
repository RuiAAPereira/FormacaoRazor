using FormacaoRazor.Models.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FormacaoRazor.Models.Marcacoes
{
    [Table("Marcacoes")]
    public class Marcacao : IBaseEntity
    {
        [Key]
        public Guid MarcacaoID { get; set; }

        //link com tabela Uh (unidades de handling)
        [Display(Name = "Unidade de handling:", ShortName = "UH:")]
        public Guid UhId { get; set; }
        public Uh Uh { get; set; }

        //link com a tabela Formacao (formações)
        [Display(Name = "Formação:", ShortName = "FOR:")]
        public Guid FormacaoId { get; set; }
        public Formacao Formacao { get; set; }

        //link com a tabela Formadores
        [Display(Name = "Formador:", ShortName = "F:")]
        public Guid FormadorID { get; set; }
        public Formador Formador { get; set; }

        //link com a tabela Salas
        public Guid SalaId { get; set; }
        public Sala Sala { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Início:", ShortName = "Início:")]
        public DateTime DataInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Fim:", ShortName = "Fim:")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Cor:", ShortName = "Cor:")]
        public string ColorCode { get; set; }

        [Display(Name = "Observações:", ShortName = "Obs:")]
        public string Observacoes { get; set; }

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
