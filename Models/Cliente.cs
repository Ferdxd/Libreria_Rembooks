using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nit {  get; set; }
        public string Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public ICollection<Venta> Ventas { get; set; }
    }
}
