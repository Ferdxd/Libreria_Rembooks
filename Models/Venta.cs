using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Monto_Recibido { get; set; }
        public decimal Cambio { get; set; }

        public int Cliente_Id { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Detalle_Venta> Detalles_Venta { get; set; }
        


    }
}
