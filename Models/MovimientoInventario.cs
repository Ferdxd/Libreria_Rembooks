using System;
namespace Libreria_Rembooks.Models
{
    public class MovimientoInventario
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = string.Empty;
        public decimal Cantidad { get; set; } // Positiva para entradas, negativa para salidas
        public decimal StockAnterior { get; set; }
        public decimal StockNuevo { get; set; }
        public string Referencia { get; set; } = string.Empty;
    }
}
