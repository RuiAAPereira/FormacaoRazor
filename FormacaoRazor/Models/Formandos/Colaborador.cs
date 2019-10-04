using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models.Formandos
{
    [Table("Colaboradores")]
    public class Colaborador : IBaseEntity
    {
        [Key]
        public Guid ColaboradorID { get; set; }
        //link com tabela Uh (unidades de handling)
        [Display(Name = "Unidade de handling:", ShortName = "UH:")]
        public Guid UhId { get; set; }
        public Uh Uh { get; set; }
        //link com tabela Departamentos
        [Display(Name = "Departamento:", ShortName = "Dpto.:")]
        public Guid DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        [Required, Display(Name = "Cartão aeroporto:", ShortName = "Cartão:")]
        public int NumCartao { get; set; }
        [Required, Display(Name = "Número portway:", ShortName = "Núm pw:")]
        public int NumPw { get; set; }

        string _nome;
        [Required, MaxLength(150), Display(Name = "Nome:")]
        public string Nome
        {
            get => _nome;
#pragma warning disable CA1062 // Validate arguments of public methods
            set => _nome = value.ToUpper(CultureInfo.InvariantCulture);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        //link com a tabela Funções
        [Display(Name = "Funções:")]
        public Guid FuncaoId { get; set; }
        public Funcao Funcao { get; set; }

        [Display(Name = "Ativo?:")]
        public bool? Ativo { get; set; }

        [Display(Name = "Registo criado em:", ShortName = "Criado em:")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Registo criado por:", ShortName = "Criado por:")]
        public string CreatedBy { get; set; }
        [Display(Name = "Registo atualizado em:", ShortName = "Atualizado em:")]
        public DateTime? LastUpdatedAt { get; set; }
        [Display(Name = "Registo atualizado por:", ShortName = "Atualizado por:")]
        public string LastUpdatedBy { get; set; }

        public ICollection<FormacaoColaborador> FormacoesColaborador { get; set; }
        public ICollection<HistoricoFormacaoColaborador> HistoricoFormacoesColaborador { get; set; }
    }
}
