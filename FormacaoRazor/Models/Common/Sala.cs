using FormacaoRazor.Models.Marcacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models.Common
{
    [Table("Salas")]
    public class Sala : IBaseEntity
    {
        [Key]
        public Guid SalaID { get; set; }

        string _salaNome;
        [Required, MaxLength(50), Display(Name = "Sala:", ShortName = "S:")]
        public string SalaNome
        {
            get => _salaNome;
#pragma warning disable CA1062 // Validate arguments of public methods
            set => _salaNome = value.ToUpper(CultureInfo.InvariantCulture);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        [Required, Display(Name = "Capacidade:")]
        public int Capacidade { get; set; }

        //link com tabela Uh (unidades de handling)
        [Display(Name = "Unidade de handling:", ShortName = "UH:")]
        public Guid UhId { get; set; }
        public Uh Uh { get; set; }

        [Display(Name = "Registo criado em:", ShortName = "Criado em:")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Registo criado por:", ShortName = "Criado por:")]
        public string CreatedBy { get; set; }
        [Display(Name = "Registo atualizado em:", ShortName = "Atualizado em:")]
        public DateTime? LastUpdatedAt { get; set; }
        [Display(Name = "Registo atualizado por:", ShortName = "Atualizado por:")]
        public string LastUpdatedBy { get; set; }

        public ICollection<Marcacao> Marcacoes { get; set; }
    }
}
