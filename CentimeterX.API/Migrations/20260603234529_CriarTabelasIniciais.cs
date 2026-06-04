using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentimeterX.API.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CX_ESTACAO_BASE",
                columns: table => new
                {
                    ID_ESTACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_CODIGO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    NM_ESTACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    NR_LATITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NR_LONGITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    FL_ONLINE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DT_ULTIMA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CX_ESTACAO_BASE", x => x.ID_ESTACAO);
                });

            migrationBuilder.CreateTable(
                name: "CX_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    TP_PERFIL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CX_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "CX_ROVER",
                columns: table => new
                {
                    ID_ROVER = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_ROVER = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TP_STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_ESTACAO_BASE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TIPO_ROVER = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    NR_AUTONOMIA_VOO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NR_ALTITUDE_MAXIMA = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NM_MODELO_TRATOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    NR_LARGURA_IMPLEMENTO = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    NR_NIVEL_AUTONOMIA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NR_VELOCIDADE_MAXIMA = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CX_ROVER", x => x.ID_ROVER);
                    table.ForeignKey(
                        name: "FK_CX_ROVER_CX_ESTACAO_BASE_ID_ESTACAO_BASE",
                        column: x => x.ID_ESTACAO_BASE,
                        principalTable: "CX_ESTACAO_BASE",
                        principalColumn: "ID_ESTACAO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CX_ROVER_CX_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "CX_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CX_OCORRENCIA",
                columns: table => new
                {
                    ID_OCORRENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_ROVER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TP_OCORRENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DS_DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    NR_LATITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NR_LONGITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    DS_FOTO_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DT_CRIADA_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CX_OCORRENCIA", x => x.ID_OCORRENCIA);
                    table.ForeignKey(
                        name: "FK_CX_OCORRENCIA_CX_ROVER_ID_ROVER",
                        column: x => x.ID_ROVER,
                        principalTable: "CX_ROVER",
                        principalColumn: "ID_ROVER",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CX_SESSAO_CORRECAO",
                columns: table => new
                {
                    ID_SESSAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_ROVER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_ESTACAO_BASE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TP_STATUS_FIX = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NR_PRECISAO_HORIZONTAL_CM = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NR_PRECISAO_VERTICAL_CM = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    DS_SISTEMA_SATELITE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DT_INICIOU_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_ENCERRADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CX_SESSAO_CORRECAO", x => x.ID_SESSAO);
                    table.ForeignKey(
                        name: "FK_CX_SESSAO_CORRECAO_CX_ESTACAO_BASE_ID_ESTACAO_BASE",
                        column: x => x.ID_ESTACAO_BASE,
                        principalTable: "CX_ESTACAO_BASE",
                        principalColumn: "ID_ESTACAO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CX_SESSAO_CORRECAO_CX_ROVER_ID_ROVER",
                        column: x => x.ID_ROVER,
                        principalTable: "CX_ROVER",
                        principalColumn: "ID_ROVER",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CX_OCORRENCIA_ID_ROVER",
                table: "CX_OCORRENCIA",
                column: "ID_ROVER");

            migrationBuilder.CreateIndex(
                name: "IX_CX_ROVER_ID_ESTACAO_BASE",
                table: "CX_ROVER",
                column: "ID_ESTACAO_BASE");

            migrationBuilder.CreateIndex(
                name: "IX_CX_ROVER_ID_USUARIO",
                table: "CX_ROVER",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_CX_SESSAO_CORRECAO_ID_ESTACAO_BASE",
                table: "CX_SESSAO_CORRECAO",
                column: "ID_ESTACAO_BASE");

            migrationBuilder.CreateIndex(
                name: "IX_CX_SESSAO_CORRECAO_ID_ROVER",
                table: "CX_SESSAO_CORRECAO",
                column: "ID_ROVER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CX_OCORRENCIA");

            migrationBuilder.DropTable(
                name: "CX_SESSAO_CORRECAO");

            migrationBuilder.DropTable(
                name: "CX_ROVER");

            migrationBuilder.DropTable(
                name: "CX_ESTACAO_BASE");

            migrationBuilder.DropTable(
                name: "CX_USUARIO");
        }
    }
}
