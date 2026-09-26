using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libreria_Rembooks.Migrations
{
    /// <inheritdoc />
    public partial class LibreriaRembooks_VersionAlpha_0_1_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Venta_Venta_Id",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Venta_Clientes_Cliente_Id",
                table: "Venta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venta",
                table: "Venta");

            migrationBuilder.RenameTable(
                name: "Venta",
                newName: "Ventas");

            migrationBuilder.RenameIndex(
                name: "IX_Venta_Cliente_Id",
                table: "Ventas",
                newName: "IX_Ventas_Cliente_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ventas",
                table: "Ventas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Ventas_Venta_Id",
                table: "DetalleVentas",
                column: "Venta_Id",
                principalTable: "Ventas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Clientes_Cliente_Id",
                table: "Ventas",
                column: "Cliente_Id",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Ventas_Venta_Id",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Clientes_Cliente_Id",
                table: "Ventas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ventas",
                table: "Ventas");

            migrationBuilder.RenameTable(
                name: "Ventas",
                newName: "Venta");

            migrationBuilder.RenameIndex(
                name: "IX_Ventas_Cliente_Id",
                table: "Venta",
                newName: "IX_Venta_Cliente_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venta",
                table: "Venta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Venta_Venta_Id",
                table: "DetalleVentas",
                column: "Venta_Id",
                principalTable: "Venta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venta_Clientes_Cliente_Id",
                table: "Venta",
                column: "Cliente_Id",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
