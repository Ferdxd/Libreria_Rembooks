using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Compra
    {
        public int Id { get; set; }//listo
        public DateTime FechaEmision { get; set; }//listo
        public string? Numero_Serie { get; set; }
        public string? Numero_Documento { get; set; }
        public string? Tipo_Documento { get; set; }
        public decimal total { get; set; } //Listo
        public DateTime FechaIngreso { get; set; }

        public int ProveedorId { get; set; }//listo
        public Proveedor? Proveedor { get; set; }
        public ICollection<Detalle_Compra> DetallesCompra { get; set; }=new List<Detalle_Compra>();

    }
}