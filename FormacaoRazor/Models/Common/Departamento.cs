using FormacaoRazor.Models.Formandos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models.Common
{
    [Table("Departamentos")]
    public partial class Departamento : IBaseEntity
    {
        [Key]
        public Guid DepartamentoId { get; set; }

        [Required, Display(Name = "Número:", ShortName = "Núm:")]
        public int DepartamentoNumero { get; set; }

        string _departamentoNome;
        [Required, MaxLength(150), Display(Name = "Departamento:", ShortName = "Dep:")]
        public string DepartamentoNome
        {
            get => _departamentoNome;
#pragma warning disable CA1062 // Validate arguments of public methods
            set => _departamentoNome = value.ToUpper(CultureInfo.InvariantCulture);
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
