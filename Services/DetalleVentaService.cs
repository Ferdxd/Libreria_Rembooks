using Libreria_Rembooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class DetalleVentaService:BaseService<Detalle_Venta>
    {
        public async Task crearDetalleVenta(int ventaId,int productoId,decimal cantidad, decimal precio_Unitario, decimal subtotal)
        {
            try
            {
                if (ventaId <= 0)
                {
                    throw new Exception($"VentaId no valido. Valor Recibido {ventaId}");
                }
                if (cantidad <= 0)
                {
                    throw new Exception($"Cantidad no valida. Valor Recibido {cantidad}");
                }
                if (subtotal <= 0)
                {
                    throw new Exception(
                        $"El subtotal debe ser mayor a 0. Valor recibido: {subtotal}"
                    );
                }

                using var db = CrearConexion();
                var detalleVenta = new Detalle_Venta()
                {
                    Venta_Id = ventaId,
                    Producto_Id = productoId,
                    Cantidad = cantidad,
                    Precio_Unitario = precio_Unitario,
                    Subtotal = subtotal
                };
                await base.Crear(detalleVenta);
            }catch(Exception ex)
            {
                throw new Exception($"Error al crear el detalle de la venta :{ex.Message}", ex);
            }
            
        }
        /*
            public int Id { get; set; }
        public int Venta_Id {  get; set; }
        public Venta Venta { get; set; }
        public int Producto_Id { get; set; }
        public Producto Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Subtotal { get; set; }
        */

    }
}
