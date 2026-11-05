using Libreria_Rembooks.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Libreria_Rembooks.Services
{
    public class ResumenProducto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public decimal GananciaEstimada => Precio - Costo;
        public decimal MargenPorcentaje => Precio > 0 ? GananciaEstimada / Precio * 100 : 0;
    }
    public class AnalisisComercialService
    {
        public async Task<List<ResumenProducto>> ObtenerResumenAsync(bool soloAlertas = false)
        {
            using var db = new appDbContext();
            var query = db.Productos.AsNoTracking().Where(p => p.Activo);
            if (soloAlertas) query = query.Where(p => p.Stock <= p.StockMinimo);
            return await query.Select(p => new ResumenProducto
            {
                ProductoId = p.Id, Nombre = p.Nombre, Stock = p.Stock,
                StockMinimo = p.StockMinimo, Costo = p.PrecioPromedio ?? p.PrecioCompra ?? 0,
                Precio = p.PrecioVenta ?? 0
            }).OrderBy(p => p.Nombre).ToListAsync();
        }
    }
}
