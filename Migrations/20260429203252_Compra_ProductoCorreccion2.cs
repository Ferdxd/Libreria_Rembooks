using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libreria_Rembooks.Migrations
{
    /// <inheritdoc />
    public partial class Compra_ProductoCorreccion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Compras",
                newName: "FechaIngreso");

            migrationBuilder.AddColumn<string>(
                name: "Marca",
                table: "Productos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioPromedio",
                table: "Productos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEmision",
                table: "Compras",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Numero_Serie",
                table: "Compras",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marca",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioPromedio",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaEmision",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "Numero_Serie",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "FechaIngreso",
                table: "Compras",
                newName: "Fecha");
        }
    }
}
