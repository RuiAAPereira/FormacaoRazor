using System;
using System.Threading;
using System.Threading.Tasks;
using FormacaoRazor.Areas.Identity.Models;
using FormacaoRazor.Models;
using FormacaoRazor.Models.Formacoes;
using FormacaoRazor.Models.Formandos;
using FormacaoRazor.Models.Marcacoes;
using FormacaoRazor.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace FormacaoRazor.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment env;
        private readonly string folderName = "Data/SeedData";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment environment)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            env = environment;
        }

        #region set DbSet

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Formacao> Formacoes { get; set; }
        public DbSet<FormacaoColaborador> FormacoesColaboradores { get; set; }
        public DbSet<HistoricoFormacaoColaborador> HistoricoFormacoesColaboradores { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Uh> Uhs { get; set; }
        public DbSet<Formador> Formadores { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public virtual DbSet<Calendario> Calendarios { get; set; }
        public DbSet<Marcacao> Marcacoes { get; set; }
        public DbSet<UserUh> UsersUhs { get; set; }
        public DbSet<UserDepartamento> UsersDepartamentos { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (builder == null) { throw new NullReferenceException(); }

            #region Calendario

            _ = builder.Entity<Calendario>(entity =>
              {
                  _ = entity.HasKey(e => e.DateKey);

                  _ = entity.Property(e => e.DateKey).ValueGeneratedNever();

                  _ = entity.Property(e => e.Date).HasColumnType("date");

                  _ = entity.Property(e => e.DaySuffix)
                      .IsRequired()
                      .HasMaxLength(2)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.DowinMonth).HasColumnName("DOWInMonth");

                  _ = entity.Property(e => e.FirstDayOfMonth).HasColumnType("date");

                  _ = entity.Property(e => e.FirstDayOfNextMonth).HasColumnType("date");

                  _ = entity.Property(e => e.FirstDayOfNextYear).HasColumnType("date");

                  _ = entity.Property(e => e.FirstDayOfQuarter).HasColumnType("date");

                  _ = entity.Property(e => e.FirstDayOfYear).HasColumnType("date");

                  _ = entity.Property(e => e.HolidayText)
                      .HasMaxLength(64)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.IsoweekOfYear).HasColumnName("ISOWeekOfYear");

                  _ = entity.Property(e => e.LastDayOfMonth).HasColumnType("date");

                  _ = entity.Property(e => e.LastDayOfQuarter).HasColumnType("date");

                  _ = entity.Property(e => e.LastDayOfYear).HasColumnType("date");

                  _ = entity.Property(e => e.Mmyyyy)
                      .IsRequired()
                      .HasColumnName("MMYYYY")
                      .HasMaxLength(6)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.MonthName)
                      .IsRequired()
                      .HasMaxLength(10)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.MonthYear)
                      .IsRequired()
                      .HasMaxLength(7)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.QuarterName)
                      .IsRequired()
                      .HasMaxLength(6)
                      .IsUnicode(false);

                  _ = entity.Property(e => e.WeekDayName)
                      .IsRequired()
                      .HasMaxLength(15)
                      .IsUnicode(false);
              });

            #endregion

            #region Colaborador

            _ = builder.Entity<Colaborador>()
                .HasIndex(c => c.Nome)
                .HasName("AlternateKey_Nome")
                .IsUnique();

            _ = builder.Entity<Colaborador>()
                 .Property(p => p.Ativo)
                .HasDefaultValue(true);

            _ = builder.Entity<Colaborador>()
                .HasOne(b => b.Uh)
                .WithMany(a => a.Colaboradores)
                .OnDelete(DeleteBehavior.Restrict);

            _ = builder.Entity<Colaborador>()
                .HasOne(b => b.Departamento)
                .WithMany(a => a.Colaboradores)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Formacao

            _ = builder.Entity<Formacao>()
                .Property(c => c.FormacaoColor)
                .HasDefaultValue("#000000");

            _ = builder.Entity<Formacao>()
                .Property(c => c.FormacaoDuracao)
                .HasDefaultValue(1);

            _ = builder.Entity<Formacao>()
                .HasIndex(c => c.FormacaoNome)
                .HasName("AlternateKey_Nome")
                .IsUnique();

            _ = builder.Entity<Formacao>()
                .HasIndex(c => c.FormacaoCod)
                .HasName("AlternateKey_Cod")
                .IsUnique();

            #endregion

            #region Marcações

            _ = builder.Entity<Marcacao>()
                .HasOne(b => b.Sala)
                .WithMany(a => a.Marcacoes)
                .OnDelete(DeleteBehavior.Restrict);

            _ = builder.Entity<Marcacao>()
                .HasOne(b => b.Uh)
                .WithMany(a => a.Marcacoes)
                .OnDelete(DeleteBehavior.Restrict);

            _ = builder.Entity<Marcacao>()
                .HasOne(b => b.Formacao)
                .WithMany(a => a.Marcacoes)
                .OnDelete(DeleteBehavior.Restrict);

            _ = builder.Entity<Marcacao>()
                .HasOne(b => b.Formador)
                .WithMany(a => a.Marcacoes)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Formador

            _ = builder.Entity<Formador>()
                .HasOne(b => b.Uh)
                .WithMany(a => a.Formadores)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Sala

            _ = builder.Entity<Sala>()
                .HasOne(b => b.Uh)
                .WithMany(a => a.Salas)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Seed Data
            // Seed Uh's
            string UhsFile = Path.Combine(env.ContentRootPath, folderName, "Uhs.json");
            FileStream UhsStream = File.Open(UhsFile, FileMode.Open, FileAccess.Read);
            using (StreamReader UhsReader = new StreamReader(UhsStream, System.Text.Encoding.UTF8))
            {
                List<Uh> _uhs = JsonConvert.DeserializeObject<List<Uh>>(UhsReader.ReadToEnd());
                _ = builder.Entity<Uh>().HasData(_uhs);
            };
            // Seed Departamentos
            string DepartamentosFile = Path.Combine(env.ContentRootPath, folderName, "Departamentos.json");
            FileStream DepartamentosStream = File.Open(DepartamentosFile, FileMode.Open, FileAccess.Read);
            using (StreamReader DepartamentosReader = new StreamReader(DepartamentosStream, System.Text.Encoding.UTF8))
            {
                List<Departamento> _departamentos = JsonConvert.DeserializeObject<List<Departamento>>(DepartamentosReader.ReadToEnd());
                _ = builder.Entity<Departamento>().HasData(_departamentos);
            };
            // Seed Funcoes
            string FuncoesFile = Path.Combine(env.ContentRootPath, folderName, "Funcoes.json");
            FileStream FuncoesStream = File.Open(FuncoesFile, FileMode.Open, FileAccess.Read);
            using (StreamReader FuncoesReader = new StreamReader(FuncoesStream, System.Text.Encoding.UTF8))
            {
                List<Funcao> _funcoes = JsonConvert.DeserializeObject<List<Funcao>>(FuncoesReader.ReadToEnd());
                _ = builder.Entity<Funcao>().HasData(_funcoes);
            };

            // Seed Salas
            string SalasFile = Path.Combine(env.ContentRootPath, folderName, "Salas.json");
            FileStream SalasStream = File.Open(SalasFile, FileMode.Open, FileAccess.Read);
            using (StreamReader SalasReader = new StreamReader(SalasStream, System.Text.Encoding.UTF8))
            {
                List<Sala> _salas = JsonConvert.DeserializeObject<List<Sala>>(SalasReader.ReadToEnd());
                _ = builder.Entity<Sala>().HasData(_salas);
            };

            // Seed Formadores
            string FormadoresFile = Path.Combine(env.ContentRootPath, folderName, "Formadores.json");
            FileStream FormadoresStream = File.Open(FormadoresFile, FileMode.Open, FileAccess.Read);
            using (StreamReader FormadoresReader = new StreamReader(FormadoresStream, System.Text.Encoding.UTF8))
            {
                List<Formador> _formadores = JsonConvert.DeserializeObject<List<Formador>>(FormadoresReader.ReadToEnd());
                _ = builder.Entity<Formador>().HasData(_formadores);
            };

            #endregion

        }

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            IEnumerable entries = ChangeTracker.Entries();
            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is IBaseEntity baseEntity)
                {
                    DateTime now = DateTime.UtcNow;
                    string user = "";
                    try
                    {
                        user = _httpContextAccessor.HttpContext.User.Identity.Name;
                    }
                    catch (NullReferenceException ex)
                    {
                        NullReferenceException erro = ex;
                        user = "SISTEMA";
                    }

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            baseEntity.LastUpdatedAt = now;
                            baseEntity.LastUpdatedBy = user;
                            break;

                        case EntityState.Added:
                            baseEntity.CreatedAt = now;
                            baseEntity.CreatedBy = user;
                            baseEntity.LastUpdatedAt = null;
                            baseEntity.LastUpdatedBy = null;
                            break;
                    }
                }
            }
        }
    }
}
