using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuApi.Migrations
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
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    DataNascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    NumeroCNH = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    TipoCNH = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Modelo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Placa = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataFimReal = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ValorDiaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ClienteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MotoId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locacoes_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cpf",
                table: "Clientes",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_NumeroCNH",
                table: "Clientes",
                column: "NumeroCNH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_ClienteId",
                table: "Locacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_MotoId",
                table: "Locacoes",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locacoes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Motos");
        }
    }
}
