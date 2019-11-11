using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FormacaoRazor.Data.Migrations
{
    public partial class UserRolesAndUh : Migration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Marcacoes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormacaoDuracao",
                table: "Formacoes",
                nullable: false,
                defaultValue: 1);

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
                name: "UsersUhs");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Marcacoes");

            migrationBuilder.DropColumn(
                name: "FormacaoDuracao",
                table: "Formacoes");
        }
    }
}
