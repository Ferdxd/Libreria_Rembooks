using System.Collections.Generic;
namespace Libreria_Rembooks.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioPromedio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVenta { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; } = 0;
        public bool Activo { get; set; } = true;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public ICollection<Detalle_Compra> DetallesCompra { get; set; } = new List<Detalle_Compra>();
        public ICollection<Detalle_Venta> DetallesVenta { get; set; } = new List<Detalle_Venta>();
        public ICollection<PresentacionProducto> Presentaciones { get; set; } = new List<PresentacionProducto>();
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
