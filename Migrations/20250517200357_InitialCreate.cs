using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRSRH_EXO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NivelRiesgo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalarioMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalarioMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PuestoId = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalarioAspirado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecomendadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatos_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PuestoId = table.Column<int>(type: "int", nullable: false),
                    SalarioMensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Capacitaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Institucion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CandidatoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capacitaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Capacitaciones_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Competencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    CandidatoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competencias_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExperienciasLaborales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PuestoOcupado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CandidatoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciasLaborales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciasLaborales_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    CandidatoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Idiomas_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PuestoId",
                table: "Candidatos",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Capacitaciones_CandidatoId",
                table: "Capacitaciones",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencias_CandidatoId",
                table: "Competencias",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasLaborales_CandidatoId",
                table: "ExperienciasLaborales",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Idiomas_CandidatoId",
                table: "Idiomas",
                column: "CandidatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capacitaciones");

            migrationBuilder.DropTable(
                name: "Competencias");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "ExperienciasLaborales");

            migrationBuilder.DropTable(
                name: "Idiomas");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Puestos");
        }
    }
}
