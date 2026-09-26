using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string Razon_Social { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public ICollection<Producto> ProductosPrincipales { get; set; }
        public ICollection<Compra> Compras { get; set; }



    }
}
