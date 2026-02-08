namespace OrderManagementAPI.DTOs
{
    public class DetalleOrdenDto
    {
        public long DetalleOrdenId { get; set; }
        public long OrdenId { get; set; }
        public long ProductoId { get; set; }
        public string? ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateDetalleOrdenDto
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
