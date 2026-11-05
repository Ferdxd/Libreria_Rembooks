using Libreria_Rembooks.Data;
using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class LineaOperacion
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; } // Siempre expresada en unidades base
        public decimal PrecioUnitario { get; set; }
    }

    public class OperacionInventarioService
    {
        public async Task<int> RegistrarVentaAsync(int clienteId, DateTime fecha, decimal montoRecibido, IEnumerable<LineaOperacion> lineas)
        {
            var items = ValidarLineas(lineas);
            decimal total = items.Sum(x => x.Cantidad * x.PrecioUnitario);
            if (clienteId <= 0) throw new ArgumentException("Seleccione un cliente válido.");
            if (montoRecibido < total) throw new InvalidOperationException("El monto recibido es insuficiente.");

            await using var db = new appDbContext();
            await using var tx = await db.Database.BeginTransactionAsync();
            // La transacción mantiene juntos encabezado, detalles y movimientos.
            // Para múltiples terminales simultáneas se necesita además coordinación de escrituras.
            var productos = await CargarProductosAsync(db, items);
            foreach (var grupo in items.GroupBy(x => x.ProductoId))
                if (productos[grupo.Key].Stock < grupo.Sum(x => x.Cantidad))
                    throw new InvalidOperationException($"Existencia insuficiente: {productos[grupo.Key].Nombre}.");

            var venta = new Venta { Cliente_Id = clienteId, Fecha = fecha, Total = total, Monto_Recibido = montoRecibido, Cambio = montoRecibido - total };
            db.Ventas.Add(venta);
            await db.SaveChangesAsync();
            foreach (var item in items)
            {
                var producto = productos[item.ProductoId];
                decimal anterior = producto.Stock;
                producto.Stock -= item.Cantidad;
                db.DetalleVentas.Add(new Detalle_Venta { Venta_Id = venta.Id, Producto_Id = item.ProductoId, Cantidad = item.Cantidad, Precio_Unitario = item.PrecioUnitario, Subtotal = item.Cantidad * item.PrecioUnitario });
                db.MovimientosInventario.Add(new MovimientoInventario { ProductoId = item.ProductoId, Fecha = DateTime.Now, Tipo = "VENTA", Cantidad = -item.Cantidad, StockAnterior = anterior, StockNuevo = producto.Stock, Referencia = $"Venta #{venta.Id}" });
            }
            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return venta.Id;
        }

        public async Task<int> RegistrarCompraAsync(int proveedorId, DateTime fechaEmision, DateTime fechaIngreso, string serie, string numero, string tipo, IEnumerable<LineaOperacion> lineas)
        {
            var items = ValidarLineas(lineas);
            if (proveedorId <= 0) throw new ArgumentException("Seleccione un proveedor válido.");
            if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(tipo)) throw new ArgumentException("Complete los datos del documento.");
            decimal total = items.Sum(x => x.Cantidad * x.PrecioUnitario);
            await using var db = new appDbContext();
            await using var tx = await db.Database.BeginTransactionAsync();
            var productos = await CargarProductosAsync(db, items);
            var compra = new Compra { ProveedorId = proveedorId, FechaEmision = fechaEmision, FechaIngreso = fechaIngreso, Numero_Serie = serie, Numero_Documento = numero, Tipo_Documento = tipo, total = total };
            db.Compras.Add(compra);
            await db.SaveChangesAsync();
            foreach (var item in items)
            {
                var producto = productos[item.ProductoId];
                decimal anterior = producto.Stock;
                decimal nuevoStock = anterior + item.Cantidad;
                decimal costoAnterior = producto.PrecioPromedio ?? producto.PrecioCompra ?? 0m;
                producto.PrecioPromedio = nuevoStock > 0 ? ((anterior * costoAnterior) + item.Cantidad * item.PrecioUnitario) / nuevoStock : item.PrecioUnitario;
                producto.PrecioCompra = item.PrecioUnitario;
                producto.Stock = nuevoStock;
                db.DetalleCompra.Add(new Detalle_Compra { Compra_Id = compra.Id, Producto_Id = item.ProductoId, Cantidad = item.Cantidad, Precio_Unitario = item.PrecioUnitario, Subtotal = item.Cantidad * item.PrecioUnitario });
                db.MovimientosInventario.Add(new MovimientoInventario { ProductoId = item.ProductoId, Fecha = DateTime.Now, Tipo = "COMPRA", Cantidad = item.Cantidad, StockAnterior = anterior, StockNuevo = nuevoStock, Referencia = $"Compra #{compra.Id}" });
            }
            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return compra.Id;
        }

        private static List<LineaOperacion> ValidarLineas(IEnumerable<LineaOperacion> lineas)
        {
            var items = lineas?.ToList() ?? throw new ArgumentNullException(nameof(lineas));
            if (items.Count == 0 || items.Any(x => x.ProductoId <= 0 || x.Cantidad <= 0 || x.PrecioUnitario <= 0))
                throw new ArgumentException("Debe ingresar productos con cantidad y precio positivos.");
            return items;
        }

        private static async Task<Dictionary<int, Producto>> CargarProductosAsync(appDbContext db, List<LineaOperacion> items)
        {
            var ids = items.Select(x => x.ProductoId).Distinct().ToList();
            var productos = await db.Productos.Where(p => ids.Contains(p.Id)).ToDictionaryAsync(p => p.Id);
            if (productos.Count != ids.Count) throw new InvalidOperationException("Uno o más productos no existen.");
            return productos;
        }
    }
}
