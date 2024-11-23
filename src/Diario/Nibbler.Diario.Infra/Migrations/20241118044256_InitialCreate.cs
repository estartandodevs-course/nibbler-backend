using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nibbler.Diario.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaminhoFoto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataDeExclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuantidadeReflexoes = table.Column<int>(type: "int", nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diarios_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiarioEtiqueta",
                columns: table => new
                {
                    DiarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EtiquetaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiarioEtiqueta", x => new { x.DiarioId, x.EtiquetaId });
                    table.ForeignKey(
                        name: "FK_DiarioEtiqueta_Diarios_DiarioId",
                        column: x => x.DiarioId,
                        principalTable: "Diarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiarioEtiqueta_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Conteudo = table.Column<string>(type: "varchar(5000)", nullable: false),
                    DataDaEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entradas_Diarios_DiarioId",
                        column: x => x.DiarioId,
                        principalTable: "Diarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reflexoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataDeAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reflexoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reflexoes_Diarios_DiarioId",
                        column: x => x.DiarioId,
                        principalTable: "Diarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reflexoes_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiarioEtiqueta_EtiquetaId",
                table: "DiarioEtiqueta",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Diarios_UsuarioId",
                table: "Diarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_DiarioId",
                table: "Entradas",
                column: "DiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reflexoes_DiarioId",
                table: "Reflexoes",
                column: "DiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reflexoes_UsuarioId",
                table: "Reflexoes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiarioEtiqueta");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropTable(
                name: "Reflexoes");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "Diarios");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
