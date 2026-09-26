using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Data
{
    public class appDbContext:DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras {  get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Detalle_Compra> DetalleCompra {  get; set; }
        public DbSet<Detalle_Venta> DetalleVentas {  get; set; }
        //produccion
        protected override void OnConfiguring(
          DbContextOptionsBuilder options)
        {
            string carpetaAplicacion = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "Libreria_Rembooks"
            );

            // Crear la carpeta si todavía no existe
            Directory.CreateDirectory(carpetaAplicacion);

            string dbPath = Path.Combine(
                carpetaAplicacion,
                "libreria.db"
            );

            options.UseSqlite(
                $"Data Source={dbPath}"
            );
        }
        /*pruebas
       protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    var projectPath = Path.GetFullPath(
        Path.Combine(Directory.GetCurrentDirectory(), @"D:\Documentos\Proyectos Cursos\Libreria_Rembooks\Database"));

    var dbPath = Path.Combine(projectPath, "libreria.db");

    options.UseSqlite($"Data Source={dbPath}");
}*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Categoria - Producto
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            // Proveedor - Compra
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany(p => p.Compras)
                .HasForeignKey(c => c.ProveedorId);

            // Compra - Detalle_Compra
            modelBuilder.Entity<Detalle_Compra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetallesCompra)
                .HasForeignKey(d => d.Compra_Id);

            // Producto - Detalle_Compra
            modelBuilder.Entity<Detalle_Compra>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesCompra)
                .HasForeignKey(d => d.Producto_Id);

            // Cliente - Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.Cliente_Id);

            // Venta - Detalle_Venta
            modelBuilder.Entity<Detalle_Venta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles_Venta)
                .HasForeignKey(d => d.Venta_Id);

            // Producto - Detalle_Venta
            modelBuilder.Entity<Detalle_Venta>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.Producto_Id);
        }



    }
}
