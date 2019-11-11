using FormacaoRazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Models.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormacaoRazor.Areas.Identity.Models
{
    public class UserAssignmentsPageModel : PageModel
    {
        #region AssignedUh
        public List<AssignedUhData> AssignedUhDataList { get; set; }

        public void PopulateAssignedUhData(ApplicationDbContext db, ApplicationUser appUser)
        {
            if (appUser == null) { throw new NullReferenceException(); }
            if (db == null) { throw new NullReferenceException(); }

            DbSet<Uh> todasUh = db.Uhs;
            HashSet<Guid> userUhs = new HashSet<Guid>(appUser.UserUhs.Select(u => u.UhId));
            AssignedUhDataList = new List<AssignedUhData>();

            foreach (Uh uh in todasUh)
            {
                AssignedUhDataList.Add(new AssignedUhData
                {
                    UhId = uh.UhId,
                    Nome = uh.IATA,
                    Assigned = userUhs.Contains(uh.UhId)
                });
            }
        }

        public void PopulateAllUhData(ApplicationDbContext db)
        {
            if (db == null) { throw new NullReferenceException(); }

            DbSet<Uh> todasUh = db.Uhs;
            AssignedUhDataList = new List<AssignedUhData>();

            foreach (Uh uh in todasUh)
            {
                AssignedUhDataList.Add(new AssignedUhData
                {
                    UhId = uh.UhId,
                    Nome = uh.IATA,
                    Assigned = false
                });
            }
        }

        public static void UpdateUserUhs(ApplicationDbContext db, string[] selectedUhs, ApplicationUser userToUpdate)
        {
            if (userToUpdate == null) { throw new NullReferenceException(); }
            if (db == null) { throw new NullReferenceException(); }
            if (selectedUhs == null)
            {
                userToUpdate.UserUhs = new List<UserUh>();
                return;
            }

            HashSet<string> selectedUhsHS = new HashSet<string>(selectedUhs);
            HashSet<Guid> userUhs = new HashSet<Guid>(userToUpdate.UserUhs.Select(u => u.UhId));
            foreach (Uh uh in db.Uhs)
            {
                if (selectedUhsHS.Contains(uh.UhId.ToString()))
                {
                    if (!userUhs.Contains(uh.UhId))
                    {
                        userToUpdate.UserUhs.Add(
                            new UserUh
                            {
                                User = userToUpdate,
                                UhId = uh.UhId
                            });
                    }
                }
                else
                {
                    if (userUhs.Contains(uh.UhId))
                    {
                        UserUh uhToRemove = userToUpdate.UserUhs.SingleOrDefault(i => i.UhId == uh.UhId);
                        _ = db.Remove(uhToRemove);
                    }
                }
            }
        }

        #endregion

        #region AssignedDepartamento
        public List<AssignedDepartamentoData> AssignedDepartamentoDataList { get; set; }

        public void PopulateAssignedDepartamentoData(ApplicationDbContext db, ApplicationUser appUser)
        {
            if (appUser == null) { throw new NullReferenceException(); }
            if (db == null) { throw new NullReferenceException(); }

            DbSet<Departamento> todosDepartamentos = db.Departamentos;
            HashSet<Guid> userDepartamentos = new HashSet<Guid>(appUser.UserDepartamento.Select(d => d.DepartamentoId));
            AssignedDepartamentoDataList = new List<AssignedDepartamentoData>();

            foreach (Departamento departamento in todosDepartamentos)
            {
                AssignedDepartamentoDataList.Add(new AssignedDepartamentoData
                {
                    DepartamentoId = departamento.DepartamentoId,
                    Nome = departamento.DepartamentoNome,
                    Assigned = userDepartamentos.Contains(departamento.DepartamentoId)
                });
            }
        }

        public void PopulateAllDepartamentoData(ApplicationDbContext db)
        {
            if (db == null) { throw new NullReferenceException(); }

            DbSet<Departamento> todosDepartamentos = db.Departamentos;
            AssignedDepartamentoDataList = new List<AssignedDepartamentoData>();

            foreach (Departamento departamento in todosDepartamentos)
            {
                AssignedDepartamentoDataList.Add(new AssignedDepartamentoData
                {
                    DepartamentoId = departamento.DepartamentoId,
                    Nome = departamento.DepartamentoNome,
                    Assigned = false
                });
            }
        }

        public static void UpdateUserDepartamentos(ApplicationDbContext db, string[] selectedDepartamentos, ApplicationUser userToUpdate)
        {
            if (userToUpdate == null) { throw new NullReferenceException(); }
            if (db == null) { throw new NullReferenceException(); }
            if (selectedDepartamentos == null)
            {
                userToUpdate.UserDepartamento = new List<UserDepartamento>();
                return;
            }

            HashSet<string> selectedDepartamentosHS = new HashSet<string>(selectedDepartamentos);
            HashSet<Guid> userDepartamentos = new HashSet<Guid>(userToUpdate.UserDepartamento.Select(d => d.DepartamentoId));
            foreach (Departamento departamento in db.Departamentos)
            {
                if (selectedDepartamentosHS.Contains(departamento.DepartamentoId.ToString()))
                {
                    if (!userDepartamentos.Contains(departamento.DepartamentoId))
                    {
                        userToUpdate.UserDepartamento.Add(
                            new UserDepartamento
                            {
                                User = userToUpdate,
                                DepartamentoId = departamento.DepartamentoId
                            });
                    }
                }
                else
                {
                    if (userDepartamentos.Contains(departamento.DepartamentoId))
                    {
                        UserDepartamento departamentoToRemove = userToUpdate.UserDepartamento.SingleOrDefault(i => i.DepartamentoId == departamento.DepartamentoId);
                        _ = db.Remove(departamentoToRemove);
                    }
                }
            }
        }
        #endregion
    }
}
