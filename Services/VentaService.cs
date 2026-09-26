using Libreria_Rembooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class VentaService:BaseService<Venta>
    {
        public async Task<int> crearVenta(int clienteId, DateTime fecha,decimal total,decimal montorecibido,decimal cambio)
        {
            using var db = CrearConexion();
            var venta = new Venta()
            {
                Cliente_Id = clienteId,
                Fecha = fecha,
                Total = total,
                Monto_Recibido = montorecibido,
                Cambio = cambio
            };
            await base.Crear(venta);
            return venta.Id;
        }

        /*   public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Monto_Recibido { get; set; }
        public decimal Cambio { get; set; }

        public int Cliente_Id { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Detalle_Venta> Detalles_Venta { get; set; }
        
*/
    }
}
