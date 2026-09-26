using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Detalle_Venta
    {
        public int Id { get; set; }
        public int Venta_Id {  get; set; }
        public Venta Venta { get; set; }
        public int Producto_Id { get; set; }
        public Producto Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
