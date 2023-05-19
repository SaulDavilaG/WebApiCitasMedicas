using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiCitasMedicas.Migrations
{
    /// <inheritdoc />
    public partial class Paciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    peso = table.Column<float>(type: "real", nullable: false),
                    altura = table.Column<float>(type: "real", nullable: false),
                    hist_medico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sexo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Medicos_MedicoID",
                        column: x => x.MedicoID,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_MedicoID",
                table: "Pacientes",
                column: "MedicoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
