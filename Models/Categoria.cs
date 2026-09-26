using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Categoria
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        public string? Descripcion {  get; set; }
        
        //relacion de uno a muchos/una categoria puede tener muchos productos
        public ICollection<Producto> Productos { get; set; }
    }
}
