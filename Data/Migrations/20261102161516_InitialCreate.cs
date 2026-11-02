using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRepair.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    NIF = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Morada = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CodigoPostal = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Localidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Especialidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoEquipamento = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Marca = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    Modelo = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    NumeroSerie = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipamentos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reparacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TecnicoId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataEntrada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DescricaoAvaria = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Diagnostico = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reparacoes_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reparacoes_Tecnicos_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Tecnicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Intervencoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReparacaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataIntervencao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DescricaoTrabalho = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    PecasUtilizadas = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TempoTrabalho = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    CustoMaoObra = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CustoPecas = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervencoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Intervencoes_Reparacoes_ReparacaoId",
                        column: x => x.ReparacaoId,
                        principalTable: "Reparacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_ClienteId",
                table: "Equipamentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Intervencoes_ReparacaoId",
                table: "Intervencoes",
                column: "ReparacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reparacoes_EquipamentoId",
                table: "Reparacoes",
                column: "EquipamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reparacoes_TecnicoId",
                table: "Reparacoes",
                column: "TecnicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Intervencoes");

            migrationBuilder.DropTable(
                name: "Reparacoes");

            migrationBuilder.DropTable(
                name: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Tecnicos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
