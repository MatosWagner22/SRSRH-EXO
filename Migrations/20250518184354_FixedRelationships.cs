using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRSRH_EXO.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capacitaciones_Candidatos_CandidatoId",
                table: "Capacitaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Competencias_Candidatos_CandidatoId",
                table: "Competencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Idiomas_Candidatos_CandidatoId",
                table: "Idiomas");

            migrationBuilder.DropIndex(
                name: "IX_Idiomas_CandidatoId",
                table: "Idiomas");

            migrationBuilder.DropIndex(
                name: "IX_Competencias_CandidatoId",
                table: "Competencias");

            migrationBuilder.DropIndex(
                name: "IX_Capacitaciones_CandidatoId",
                table: "Capacitaciones");

            migrationBuilder.DropColumn(
                name: "CandidatoId",
                table: "Idiomas");

            migrationBuilder.DropColumn(
                name: "CandidatoId",
                table: "Competencias");

            migrationBuilder.DropColumn(
                name: "CandidatoId",
                table: "Capacitaciones");

            migrationBuilder.CreateTable(
                name: "CandidatoCapacitaciones",
                columns: table => new
                {
                    CandidatosId = table.Column<int>(type: "int", nullable: false),
                    CapacitacionesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatoCapacitaciones", x => new { x.CandidatosId, x.CapacitacionesId });
                    table.ForeignKey(
                        name: "FK_CandidatoCapacitaciones_Candidatos_CandidatosId",
                        column: x => x.CandidatosId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatoCapacitaciones_Capacitaciones_CapacitacionesId",
                        column: x => x.CapacitacionesId,
                        principalTable: "Capacitaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidatoCompetencias",
                columns: table => new
                {
                    CandidatosId = table.Column<int>(type: "int", nullable: false),
                    CompetenciasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatoCompetencias", x => new { x.CandidatosId, x.CompetenciasId });
                    table.ForeignKey(
                        name: "FK_CandidatoCompetencias_Candidatos_CandidatosId",
                        column: x => x.CandidatosId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatoCompetencias_Competencias_CompetenciasId",
                        column: x => x.CompetenciasId,
                        principalTable: "Competencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidatoIdiomas",
                columns: table => new
                {
                    CandidatosId = table.Column<int>(type: "int", nullable: false),
                    IdiomasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatoIdiomas", x => new { x.CandidatosId, x.IdiomasId });
                    table.ForeignKey(
                        name: "FK_CandidatoIdiomas_Candidatos_CandidatosId",
                        column: x => x.CandidatosId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatoIdiomas_Idiomas_IdiomasId",
                        column: x => x.IdiomasId,
                        principalTable: "Idiomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidatoCapacitaciones_CapacitacionesId",
                table: "CandidatoCapacitaciones",
                column: "CapacitacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatoCompetencias_CompetenciasId",
                table: "CandidatoCompetencias",
                column: "CompetenciasId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatoIdiomas_IdiomasId",
                table: "CandidatoIdiomas",
                column: "IdiomasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatoCapacitaciones");

            migrationBuilder.DropTable(
                name: "CandidatoCompetencias");

            migrationBuilder.DropTable(
                name: "CandidatoIdiomas");

            migrationBuilder.AddColumn<int>(
                name: "CandidatoId",
                table: "Idiomas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidatoId",
                table: "Competencias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidatoId",
                table: "Capacitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Idiomas_CandidatoId",
                table: "Idiomas",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencias_CandidatoId",
                table: "Competencias",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Capacitaciones_CandidatoId",
                table: "Capacitaciones",
                column: "CandidatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Capacitaciones_Candidatos_CandidatoId",
                table: "Capacitaciones",
                column: "CandidatoId",
                principalTable: "Candidatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competencias_Candidatos_CandidatoId",
                table: "Competencias",
                column: "CandidatoId",
                principalTable: "Candidatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Idiomas_Candidatos_CandidatoId",
                table: "Idiomas",
                column: "CandidatoId",
                principalTable: "Candidatos",
                principalColumn: "Id");
        }
    }
}
