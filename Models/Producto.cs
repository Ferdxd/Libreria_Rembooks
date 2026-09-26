using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Nombre {  get; set; }
        public string? Marca { get; set; }
        public string? Descripcion {  get; set; }
        public decimal? PrecioPromedio { get; set; }
        public decimal? PrecioCompra {  get; set; }
        public decimal? PrecioVenta {  get; set; }
        public string UnidadMedida { get; set; }
        public decimal Stock { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public ICollection<Detalle_Compra> DetallesCompra {  get; set; }
        public ICollection<Detalle_Venta> DetallesVenta { get; set; }
    }
}
