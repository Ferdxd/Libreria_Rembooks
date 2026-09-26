using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class DetalleCompraService:BaseService<Detalle_Compra>
    {
        public async Task crearDetalleCompra(int compraId, int productoId,decimal cantidad, decimal precioUnitario, decimal subtotal)
        {
         
            using var db = CrearConexion();
            var detalleCompra = new Detalle_Compra()
            {
                Compra_Id = compraId,
                Producto_Id = productoId,
                Cantidad = cantidad,
                Precio_Unitario = precioUnitario,
                Subtotal = subtotal
            };
            await base.Crear(detalleCompra);
        }

        public async Task<List<Detalle_Compra>> obtenerDetalleCompraxId(int id)
        {
            using var db = CrearConexion();

            return await db.DetalleCompra.Where(c => c.Compra_Id == id)
                .Include(c=>c.Producto)
                .ToListAsync();
        }


        

    }
}
