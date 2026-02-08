namespace OrderManagementAPI.DTOs
{
    public class OrdenDto
    {
        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DetalleOrdenDto>? Detalles { get; set; }
    }

    public class CreateOrdenDto
    {
        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public List<CreateDetalleOrdenDto> Detalles { get; set; } = new List<CreateDetalleOrdenDto>();
    }

    public class UpdateOrdenDto
    {
        public long ClienteId { get; set; }
        public List<CreateDetalleOrdenDto> Detalles { get; set; } = new List<CreateDetalleOrdenDto>();
    }
}
