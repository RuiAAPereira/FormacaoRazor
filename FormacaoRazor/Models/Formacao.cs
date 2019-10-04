using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models.Marcacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models
{
    [Table("Formacoes")]
    public partial class Formacao : IBaseEntity
    {
        public Formacao()
        {
            FormacoesColaborador = new HashSet<FormacaoColaborador>();
        }

        [Key]
        public Guid FormacaoId { get; set; }

        string _formacaoNome;
        [Required, Display(Name = "Formação:", ShortName = "Nome.:")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public string FormacaoNome
        {
            get => _formacaoNome;
            set => _formacaoNome = value.ToUpper(CultureInfo.InvariantCulture);
        }

        string _formacaoCod;
        [Required, Display(Name = "Código:", ShortName = "Cod.:")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public string FormacaoCod
        {
            get => _formacaoCod;
            set => _formacaoCod = value.ToUpper(CultureInfo.InvariantCulture);
        }

        [Required, Display(Name = "Duração:", ShortName = "Dur:")]
        public int FormacaoDuracao { get; set; }
        [Required, Display(Name = "Validade:", ShortName = "Val:")]
        public int FormacaoValidade { get; set; }
        [Display(Name = "Cor:", ShortName = "Cor:")]
        public string FormacaoColor { get; set; }

        [Display(Name = "Registo criado em:", ShortName = "Criado em:")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Registo criado por:", ShortName = "Criado por:")]
        public string CreatedBy { get; set; }
        [Display(Name = "Registo atualizado em:", ShortName = "Atualizado em:")]
        public DateTime? LastUpdatedAt { get; set; }
        [Display(Name = "Registo atualizado por:", ShortName = "Atualizado por:")]
        public string LastUpdatedBy { get; set; }

        public ICollection<FormacoesFormador> FormacoesFormadores { get; set; }
        public ICollection<FormacaoColaborador> FormacoesColaborador { get; set; }
        public ICollection<HistoricoFormacaoColaborador> HistoricoFormacoesColaborador { get; set; }
        public ICollection<Marcacao> Marcacoes { get; set; }
    }
}
