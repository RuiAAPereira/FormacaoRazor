using FormacaoRazor.Models.Formandos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models.Common
{
    [Table("Funcoes")]
    public class Funcao : IBaseEntity
    {
        [Key]
        public Guid FuncaoId { get; set; }

        string _funcaoNome;
        [Required, MaxLength(50), Display(Name = "Função:")]
        public string FuncaoNome
        {
            get => _funcaoNome;
#pragma warning disable CA1062 // Validate arguments of public methods
            set => _funcaoNome = value.ToUpper(CultureInfo.InvariantCulture);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        [Display(Name = "Registo criado em:", ShortName = "Criado em:")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Registo criado por:", ShortName = "Criado por:")]
        public string CreatedBy { get; set; }
        [Display(Name = "Registo atualizado em:", ShortName = "Atualizado em:")]
        public DateTime? LastUpdatedAt { get; set; }
        [Display(Name = "Registo atualizado por:", ShortName = "Atualizado por:")]
        public string LastUpdatedBy { get; set; }

        public ICollection<Colaborador> Colaboradores { get; set; }
    }
}
