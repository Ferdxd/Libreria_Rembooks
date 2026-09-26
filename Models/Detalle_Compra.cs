using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Detalle_Compra
    {
        public int Id { get; set; }
        public int Compra_Id { get; set; }
        public Compra? Compra { get; set; }
        public int Producto_Id { get; set; }
        public Producto? Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
