using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
namespace Libreria_Rembooks.Data
{
    public class appDbContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Detalle_Compra> DetalleCompra { get; set; }
        public DbSet<Detalle_Venta> DetalleVentas { get; set; }
        public DbSet<PresentacionProducto> PresentacionesProducto { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Libreria_Rembooks");
            Directory.CreateDirectory(carpeta);
            options.UseSqlite($"Data Source={Path.Combine(carpeta, "libreria.db")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().HasOne(p => p.Categoria).WithMany(c => c.Productos).HasForeignKey(p => p.CategoriaId);
            modelBuilder.Entity<Compra>().HasOne(c => c.Proveedor).WithMany(p => p.Compras).HasForeignKey(c => c.ProveedorId);
            modelBuilder.Entity<Detalle_Compra>().HasOne(d => d.Compra).WithMany(c => c.DetallesCompra).HasForeignKey(d => d.Compra_Id);
            modelBuilder.Entity<Detalle_Compra>().HasOne(d => d.Producto).WithMany(p => p.DetallesCompra).HasForeignKey(d => d.Producto_Id);
            modelBuilder.Entity<Venta>().HasOne(v => v.Cliente).WithMany(c => c.Ventas).HasForeignKey(v => v.Cliente_Id);
            modelBuilder.Entity<Detalle_Venta>().HasOne(d => d.Venta).WithMany(v => v.Detalles_Venta).HasForeignKey(d => d.Venta_Id);
            modelBuilder.Entity<Detalle_Venta>().HasOne(d => d.Producto).WithMany(p => p.DetallesVenta).HasForeignKey(d => d.Producto_Id);
            modelBuilder.Entity<PresentacionProducto>().HasOne(p => p.Producto).WithMany(p => p.Presentaciones).HasForeignKey(p => p.ProductoId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MovimientoInventario>().HasOne(m => m.Producto).WithMany(p => p.Movimientos).HasForeignKey(m => m.ProductoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PresentacionProducto>().HasIndex(p => p.CodigoBarras).IsUnique();
            modelBuilder.Entity<MovimientoInventario>().HasIndex(m => new { m.ProductoId, m.Fecha });
        }
    }
}
