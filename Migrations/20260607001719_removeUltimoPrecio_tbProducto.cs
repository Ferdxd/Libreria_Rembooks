using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libreria_Rembooks.Migrations
{
    /// <inheritdoc />
    public partial class removeUltimoPrecio_tbProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimoPrecio",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UltimoPrecio",
                table: "Productos",
                type: "TEXT",
                nullable: true);
        }
    }
}
