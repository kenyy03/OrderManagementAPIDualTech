namespace OrderManagementAPI.Models
{
    public class DetalleOrden
    {
        public long DetalleOrdenId { get; set; }
        public long OrdenId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        public Orden Orden { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}
