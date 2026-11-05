namespace Libreria_Rembooks.Models
{
    public class PresentacionProducto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        public string Nombre { get; set; } = string.Empty;
        public decimal UnidadesPorPresentacion { get; set; } = 1;
        public decimal PrecioVenta { get; set; }
        public string? CodigoBarras { get; set; }
        public bool Activa { get; set; } = true;
    }
}
