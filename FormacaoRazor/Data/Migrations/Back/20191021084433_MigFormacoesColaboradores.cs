using Microsoft.EntityFrameworkCore.Migrations;

namespace FormacaoRazor.Data.Migrations
{
    public partial class MigFormacoesColaboradores : Migration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "RefreshRequired",
                table: "HistoricoFormacoesColaboradores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefreshRequired",
                table: "FormacoesColaboradores",
                nullable: false,
                defaultValue: false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RefreshRequired",
                table: "HistoricoFormacoesColaboradores");

            migrationBuilder.DropColumn(
                name: "RefreshRequired",
                table: "FormacoesColaboradores");
        }
    }
}
