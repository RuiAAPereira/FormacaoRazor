using FormacaoRazor.Models.Formandos;
using FormacaoRazor.Models.Marcacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FormacaoRazor.Models.Common
{
    [Table("Uhs")]
    public class Uh : IBaseEntity
    {
        [Key]
        public Guid UhId { get; set; }
        [Required, MaxLength(5), Display(Name = "Código IATA:", ShortName = "IATA:")]
        public string IATA { get; set; }

        string _uhNome;
        [Required, MaxLength(25), Display(Name = "Unidade de handling:", ShortName = "UH:")]
        public string UhNome
        {

            get => _uhNome;
#pragma warning disable CA1062 // Validate arguments of public methods
            set => _uhNome = value.ToUpper(CultureInfo.InvariantCulture);
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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ICollection<Colaborador> Colaboradores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ICollection<Sala> Salas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ICollection<Formador> Formadores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ICollection<Marcacao> Marcacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ICollection<Departamento> Departamentos { get; set; }

    }
}
