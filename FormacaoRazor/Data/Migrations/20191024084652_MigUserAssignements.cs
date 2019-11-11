using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FormacaoRazor.Data.Migrations
{
    public partial class MigUserAssignements : Migration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_Uhs_UhId",
                table: "Departamentos");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_UhId",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "UhId",
                table: "Departamentos");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Marcacoes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RefreshRequired",
                table: "HistoricoFormacoesColaboradores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefreshRequired",
                table: "FormacoesColaboradores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FormacaoDuracao",
                table: "Formacoes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Colaboradores",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "UsersDepartamentos",
                columns: table => new
                {
                    UserDepartamentoId = table.Column<Guid>(nullable: false),
                    DepartamentoId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDepartamentos", x => x.UserDepartamentoId);
                    table.ForeignKey(
                        name: "FK_UsersDepartamentos_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "DepartamentoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersDepartamentos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersUhs",
                columns: table => new
                {
                    UserUhId = table.Column<Guid>(nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersUhs", x => x.UserUhId);
                    table.ForeignKey(
                        name: "FK_UsersUhs_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersUhs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersDepartamentos_DepartamentoId",
                table: "UsersDepartamentos",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDepartamentos_UserId",
                table: "UsersDepartamentos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersUhs_UhId",
                table: "UsersUhs",
                column: "UhId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersUhs_UserId",
                table: "UsersUhs",
                column: "UserId");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "UsersDepartamentos");

            migrationBuilder.DropTable(
                name: "UsersUhs");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Marcacoes");

            migrationBuilder.DropColumn(
                name: "RefreshRequired",
                table: "HistoricoFormacoesColaboradores");

            migrationBuilder.DropColumn(
                name: "RefreshRequired",
                table: "FormacoesColaboradores");

            migrationBuilder.DropColumn(
                name: "FormacaoDuracao",
                table: "Formacoes");

            migrationBuilder.AddColumn<Guid>(
                name: "UhId",
                table: "Departamentos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Colaboradores",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "Departamentos",
                keyColumn: "DepartamentoId",
                keyValue: new Guid("3d4b7e78-fe91-4fca-93ba-8492ab5a47e0"),
                column: "UhId",
                value: new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140"));

            migrationBuilder.UpdateData(
                table: "Departamentos",
                keyColumn: "DepartamentoId",
                keyValue: new Guid("f18e3fcd-adb3-4136-a31b-9f9cc9b93701"),
                column: "UhId",
                value: new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140"));

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_UhId",
                table: "Departamentos",
                column: "UhId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_Uhs_UhId",
                table: "Departamentos",
                column: "UhId",
                principalTable: "Uhs",
                principalColumn: "UhId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
