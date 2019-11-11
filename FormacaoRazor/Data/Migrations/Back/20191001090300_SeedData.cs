using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FormacaoRazor.Data.Migrations
{
    public partial class SeedData : Migration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Calendarios",
                columns: table => new
                {
                    DateKey = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Day = table.Column<byte>(nullable: false),
                    DaySuffix = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Weekday = table.Column<byte>(nullable: false),
                    WeekDayName = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    IsWeekend = table.Column<bool>(nullable: false),
                    IsHoliday = table.Column<bool>(nullable: false),
                    HolidayText = table.Column<string>(unicode: false, maxLength: 64, nullable: true),
                    DOWInMonth = table.Column<byte>(nullable: false),
                    DayOfYear = table.Column<short>(nullable: false),
                    WeekOfMonth = table.Column<byte>(nullable: false),
                    WeekOfYear = table.Column<byte>(nullable: false),
                    ISOWeekOfYear = table.Column<byte>(nullable: false),
                    Month = table.Column<byte>(nullable: false),
                    MonthName = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Quarter = table.Column<byte>(nullable: false),
                    QuarterName = table.Column<string>(unicode: false, maxLength: 6, nullable: false),
                    Year = table.Column<int>(nullable: false),
                    MMYYYY = table.Column<string>(unicode: false, maxLength: 6, nullable: false),
                    MonthYear = table.Column<string>(unicode: false, maxLength: 7, nullable: false),
                    FirstDayOfMonth = table.Column<DateTime>(type: "date", nullable: false),
                    LastDayOfMonth = table.Column<DateTime>(type: "date", nullable: false),
                    FirstDayOfQuarter = table.Column<DateTime>(type: "date", nullable: false),
                    LastDayOfQuarter = table.Column<DateTime>(type: "date", nullable: false),
                    FirstDayOfYear = table.Column<DateTime>(type: "date", nullable: false),
                    LastDayOfYear = table.Column<DateTime>(type: "date", nullable: false),
                    FirstDayOfNextMonth = table.Column<DateTime>(type: "date", nullable: false),
                    FirstDayOfNextYear = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendarios", x => x.DateKey);
                });

            migrationBuilder.CreateTable(
                name: "Formacoes",
                columns: table => new
                {
                    FormacaoId = table.Column<Guid>(nullable: false),
                    FormacaoNome = table.Column<string>(nullable: false),
                    FormacaoCod = table.Column<string>(nullable: false),
                    FormacaoValidade = table.Column<int>(nullable: false),
                    FormacaoColor = table.Column<string>(nullable: true, defaultValue: "#000000"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formacoes", x => x.FormacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    FuncaoId = table.Column<Guid>(nullable: false),
                    FuncaoNome = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.FuncaoId);
                });

            migrationBuilder.CreateTable(
                name: "Uhs",
                columns: table => new
                {
                    UhId = table.Column<Guid>(nullable: false),
                    IATA = table.Column<string>(maxLength: 5, nullable: false),
                    UhNome = table.Column<string>(maxLength: 25, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uhs", x => x.UhId);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    DepartamentoId = table.Column<Guid>(nullable: false),
                    DepartamentoNumero = table.Column<int>(nullable: false),
                    DepartamentoNome = table.Column<string>(maxLength: 150, nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.DepartamentoId);
                    table.ForeignKey(
                        name: "FK_Departamentos_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Formadores",
                columns: table => new
                {
                    FormadorID = table.Column<Guid>(nullable: false),
                    FormadorNome = table.Column<string>(maxLength: 50, nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formadores", x => x.FormadorID);
                    table.ForeignKey(
                        name: "FK_Formadores_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    SalaID = table.Column<Guid>(nullable: false),
                    SalaNome = table.Column<string>(maxLength: 50, nullable: false),
                    Capacidade = table.Column<int>(nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.SalaID);
                    table.ForeignKey(
                        name: "FK_Salas_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                columns: table => new
                {
                    ColaboradorID = table.Column<Guid>(nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    DepartamentoId = table.Column<Guid>(nullable: false),
                    NumCartao = table.Column<int>(nullable: false),
                    NumPw = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(maxLength: 150, nullable: false),
                    FuncaoId = table.Column<Guid>(nullable: false),
                    Ativo = table.Column<bool>(nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaboradores", x => x.ColaboradorID);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "DepartamentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "FuncaoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormacoesFormadores",
                columns: table => new
                {
                    FormacoesFormadorId = table.Column<Guid>(nullable: false),
                    FormacaoId = table.Column<Guid>(nullable: false),
                    FormadorId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormacoesFormadores", x => x.FormacoesFormadorId);
                    table.ForeignKey(
                        name: "FK_FormacoesFormadores_Formacoes_FormacaoId",
                        column: x => x.FormacaoId,
                        principalTable: "Formacoes",
                        principalColumn: "FormacaoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormacoesFormadores_Formadores_FormadorId",
                        column: x => x.FormadorId,
                        principalTable: "Formadores",
                        principalColumn: "FormadorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marcacoes",
                columns: table => new
                {
                    MarcacaoID = table.Column<Guid>(nullable: false),
                    UhId = table.Column<Guid>(nullable: false),
                    FormacaoId = table.Column<Guid>(nullable: false),
                    FormadorID = table.Column<Guid>(nullable: false),
                    SalaId = table.Column<Guid>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: false),
                    ColorCode = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcacoes", x => x.MarcacaoID);
                    table.ForeignKey(
                        name: "FK_Marcacoes_Formacoes_FormacaoId",
                        column: x => x.FormacaoId,
                        principalTable: "Formacoes",
                        principalColumn: "FormacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marcacoes_Formadores_FormadorID",
                        column: x => x.FormadorID,
                        principalTable: "Formadores",
                        principalColumn: "FormadorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marcacoes_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "SalaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marcacoes_Uhs_UhId",
                        column: x => x.UhId,
                        principalTable: "Uhs",
                        principalColumn: "UhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormacoesColaboradores",
                columns: table => new
                {
                    FormacaoColaboradorId = table.Column<Guid>(nullable: false),
                    FormacaoId = table.Column<Guid>(nullable: false),
                    ColaboradorId = table.Column<Guid>(nullable: false),
                    FormacaoData = table.Column<DateTime>(nullable: false),
                    FormacaoCaducidade = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormacoesColaboradores", x => x.FormacaoColaboradorId);
                    table.ForeignKey(
                        name: "FK_FormacoesColaboradores_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "ColaboradorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormacoesColaboradores_Formacoes_FormacaoId",
                        column: x => x.FormacaoId,
                        principalTable: "Formacoes",
                        principalColumn: "FormacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoFormacoesColaboradores",
                columns: table => new
                {
                    HistoricoFormacaoColaboradorId = table.Column<Guid>(nullable: false),
                    FormacaoId = table.Column<Guid>(nullable: false),
                    ColaboradorId = table.Column<Guid>(nullable: false),
                    FormacaoData = table.Column<DateTime>(nullable: false),
                    FormacaoCaducidade = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoFormacoesColaboradores", x => x.HistoricoFormacaoColaboradorId);
                    table.ForeignKey(
                        name: "FK_HistoricoFormacoesColaboradores_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "ColaboradorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoFormacoesColaboradores_Formacoes_FormacaoId",
                        column: x => x.FormacaoId,
                        principalTable: "Formacoes",
                        principalColumn: "FormacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "Funcoes",
                columns: new[] { "FuncaoId", "CreatedAt", "CreatedBy", "FuncaoNome", "LastUpdatedAt", "LastUpdatedBy" },
                values: new object[,]
                {
                    { new Guid("c4e8e4a9-e6c0-4298-8c05-d88bfdc83b8c"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "COORDENADOR", null, null },
                    { new Guid("7ac27970-8e3e-4ee3-8f07-33467953d9ab"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ADMINISTRATIVO OPS", null, null },
                    { new Guid("b33efecd-cc02-42e4-ae45-23061d46901c"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "COORDENAÇÃO LOAD CONTROL", null, null },
                    { new Guid("dd6d7f1a-dfa4-46d5-9735-7f30307de8bb"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "DISPATCHER", null, null },
                    { new Guid("165c7875-bc22-4497-8314-74d45ad99bb3"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "LOAD CONTROL", null, null },
                    { new Guid("fd00f2ea-8f25-4fff-be66-0cb0f5b7aeb7"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "COORDENAÇÃO OPS", null, null },
                    { new Guid("d27cf5ef-dd00-4500-acff-0d284b010400"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "PLANEAMENTO", null, null },
                    { new Guid("549936c3-26f2-45ff-971b-e1fa4d493e26"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "QUALIDADE", null, null },
                    { new Guid("d0e3ba3d-aa62-48a5-9050-786b00950479"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "MOTORISTA MYWAY", null, null },
                    { new Guid("ee0cea92-ffd6-4168-8b17-8f2f3207b618"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ASSISTENTE PMR PART-TIME", null, null },
                    { new Guid("e8b0d9fc-65ac-4614-beed-7a62d983d8c8"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ASSISTENTE PMR", null, null },
                    { new Guid("a2e3aba8-4b04-41da-a585-5d48ae03c066"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ALOCADOR MYWAY", null, null },
                    { new Guid("78c4e1bd-9b1b-4ff3-91d4-94e2139ff569"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "SUPERVISOR MYWAY", null, null },
                    { new Guid("79238b38-4ea8-4d3d-9358-b28d13ab5008"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ASSISTENTE ADMINISTRATIVA", null, null },
                    { new Guid("5454ff34-6fd6-4ef6-b21f-ed52b2f20599"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "MOTORISTA AMBULIFT", null, null }
                });

            migrationBuilder.InsertData(
                table: "Uhs",
                columns: new[] { "UhId", "CreatedAt", "CreatedBy", "IATA", "LastUpdatedAt", "LastUpdatedBy", "UhNome" },
                values: new object[,]
                {
                    { new Guid("b8040251-73a1-4dbd-8af3-5526ace9f710"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "FAO", null, null, "FARO" },
                    { new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "LIS", null, null, "LISBOA" },
                    { new Guid("57f7b1cc-c94f-4b19-871f-ee5d2e4184af"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "OPO", null, null, "PORTO" },
                    { new Guid("660d1f5e-475b-482f-b140-c8c825e971ad"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "FNC", null, null, "FUNCHAL" }
                });

            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "DepartamentoId", "CreatedAt", "CreatedBy", "DepartamentoNome", "DepartamentoNumero", "LastUpdatedAt", "LastUpdatedBy", "UhId" },
                values: new object[,]
                {
                    { new Guid("3d4b7e78-fe91-4fca-93ba-8492ab5a47e0"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "PMR'S", 71680, null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("f18e3fcd-adb3-4136-a31b-9f9cc9b93701"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "OPERAÇÕES", 70001, null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") }
                });

            migrationBuilder.InsertData(
                table: "Formadores",
                columns: new[] { "FormadorID", "CreatedAt", "CreatedBy", "FormadorNome", "LastUpdatedAt", "LastUpdatedBy", "UhId" },
                values: new object[,]
                {
                    { new Guid("18072b41-1652-4b1c-bf17-22785670bdaa"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "RUI PEREIRA", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("8b67f507-daff-4d56-92b3-f4fa61d69926"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "MÁRIO ANTÓNIO", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("3dccb2aa-00c0-4309-b5fe-7eceb47c4c98"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "JOÃO DOMINGUES", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("9f15f2b6-27c7-4731-ab28-9761ced3539d"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "ARTUR AFONSO", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("c7f578db-468e-4707-9aaa-4eff8e1acb61"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "RAQUEL LEITÃO", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("bcd988f0-3373-4921-9a1d-dde8a607a165"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "IURI ISIDORO", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("b0d74e71-bdc8-4a7b-9ed6-f4c592b16b5c"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "VITOR LANDIM", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("ba6e4f7a-5f4f-499e-9c28-4646414443b2"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "HELENA LEITÃO", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("9ebf511c-0cd7-4f85-b9bc-0c3678cd15b4"), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", "FILIPE MARQUES", null, null, new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") }
                });

            migrationBuilder.InsertData(
                table: "Salas",
                columns: new[] { "SalaID", "Capacidade", "CreatedAt", "CreatedBy", "LastUpdatedAt", "LastUpdatedBy", "SalaNome", "UhId" },
                values: new object[,]
                {
                    { new Guid("463db092-7c78-4509-83fe-76e62baf677c"), 22, new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", null, null, "SALA 1 - EDIFÍCIO 124", new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("7fd4ba6e-267b-4a42-89bc-722c9d6003a9"), 15, new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", null, null, "SALA 2 - EDIFÍCIO 124, 2�PISO", new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("aae68aa3-1af8-4504-a1c2-706fabbed357"), 10, new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", null, null, "SALA 3 - EDIFÍCIO 124, PC'S", new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") },
                    { new Guid("7c5593fc-d0ef-494f-b2c6-0f4ccf471a6f"), 22, new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SISTEMA", null, null, "SALA NOVA 10", new Guid("04fde6b2-fd97-4856-9640-b9e1abc73140") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_DepartamentoId",
                table: "Colaboradores",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_FuncaoId",
                table: "Colaboradores",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "AlternateKey_Nome",
                table: "Colaboradores",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_UhId",
                table: "Colaboradores",
                column: "UhId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_UhId",
                table: "Departamentos",
                column: "UhId");

            migrationBuilder.CreateIndex(
                name: "AlternateKey_Cod",
                table: "Formacoes",
                column: "FormacaoCod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AlternateKey_Nome",
                table: "Formacoes",
                column: "FormacaoNome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormacoesColaboradores_ColaboradorId",
                table: "FormacoesColaboradores",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_FormacoesColaboradores_FormacaoId",
                table: "FormacoesColaboradores",
                column: "FormacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormacoesFormadores_FormacaoId",
                table: "FormacoesFormadores",
                column: "FormacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormacoesFormadores_FormadorId",
                table: "FormacoesFormadores",
                column: "FormadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Formadores_UhId",
                table: "Formadores",
                column: "UhId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoFormacoesColaboradores_ColaboradorId",
                table: "HistoricoFormacoesColaboradores",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoFormacoesColaboradores_FormacaoId",
                table: "HistoricoFormacoesColaboradores",
                column: "FormacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacoes_FormacaoId",
                table: "Marcacoes",
                column: "FormacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacoes_FormadorID",
                table: "Marcacoes",
                column: "FormadorID");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacoes_SalaId",
                table: "Marcacoes",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacoes_UhId",
                table: "Marcacoes",
                column: "UhId");

            migrationBuilder.CreateIndex(
                name: "IX_Salas_UhId",
                table: "Salas",
                column: "UhId");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Calendarios");

            migrationBuilder.DropTable(
                name: "FormacoesColaboradores");

            migrationBuilder.DropTable(
                name: "FormacoesFormadores");

            migrationBuilder.DropTable(
                name: "HistoricoFormacoesColaboradores");

            migrationBuilder.DropTable(
                name: "Marcacoes");

            migrationBuilder.DropTable(
                name: "Colaboradores");

            migrationBuilder.DropTable(
                name: "Formacoes");

            migrationBuilder.DropTable(
                name: "Formadores");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "Uhs");
        }
    }
}
