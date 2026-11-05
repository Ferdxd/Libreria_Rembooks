using Libreria_Rembooks.Data;
using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Libreria_Rembooks.Services
{
    public class PresentacionProductoService
    {
        public async Task<List<PresentacionProducto>> ListarAsync(int productoId)
        {
            using var db = new appDbContext();
            return await db.PresentacionesProducto.AsNoTracking().Where(x => x.ProductoId == productoId).OrderBy(x => x.Nombre).ToListAsync();
        }
        public async Task<int> GuardarAsync(PresentacionProducto dato)
        {
            if (dato.ProductoId <= 0 || string.IsNullOrWhiteSpace(dato.Nombre) || dato.UnidadesPorPresentacion <= 0 || dato.PrecioVenta <= 0)
                throw new ArgumentException("Presentación inválida.");
            using var db = new appDbContext();
            if (!await db.Productos.AnyAsync(p => p.Id == dato.ProductoId)) throw new InvalidOperationException("Producto inexistente.");
            if (dato.Id == 0) db.PresentacionesProducto.Add(dato);
            else
            {
                var actual = await db.PresentacionesProducto.FindAsync(dato.Id) ?? throw new InvalidOperationException("Presentación inexistente.");
                if (actual.ProductoId != dato.ProductoId) throw new InvalidOperationException("No se puede cambiar el producto asociado.");
                actual.Nombre = dato.Nombre;
                actual.UnidadesPorPresentacion = dato.UnidadesPorPresentacion;
                actual.PrecioVenta = dato.PrecioVenta;
                actual.CodigoBarras = dato.CodigoBarras;
                actual.Activa = dato.Activa;
            }
            await db.SaveChangesAsync();
            return dato.Id;
        }
        public async Task<LineaOperacion> CrearLineaVentaAsync(int presentacionId, decimal cantidadPresentaciones)
        {
            if (cantidadPresentaciones <= 0) throw new ArgumentException("Cantidad inválida.");
            using var db = new appDbContext();
            var p = await db.PresentacionesProducto.AsNoTracking().FirstOrDefaultAsync(x => x.Id == presentacionId && x.Activa)
                ?? throw new InvalidOperationException("Presentación no disponible.");
            // Precio unitario equivalente; el subtotal queda: presentaciones * precio presentación.
            return new LineaOperacion { ProductoId = p.ProductoId, Cantidad = cantidadPresentaciones * p.UnidadesPorPresentacion, PrecioUnitario = p.PrecioVenta / p.UnidadesPorPresentacion };
        }
    }
}
