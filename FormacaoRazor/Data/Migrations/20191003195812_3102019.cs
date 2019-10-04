using Microsoft.EntityFrameworkCore.Migrations;

namespace FormacaoRazor.Data.Migrations
{
    public partial class _3102019 : Migration
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Marcacoes");

            migrationBuilder.DropColumn(
                name: "FormacaoDuracao",
                table: "Formacoes");
        }
    }
}
