using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiCitasMedicas.Migrations
{
    /// <inheritdoc />
    public partial class Modificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Medicos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Medicos");
        }
    }
}
