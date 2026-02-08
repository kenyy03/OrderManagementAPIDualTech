namespace OrderManagementAPI.DTOs
{
    public class ClienteDto
    {
        public long ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Identidad { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateClienteDto
    {
        public long ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Identidad { get; set; } = null!;
    }

    public class UpdateClienteDto
    {
        public string Nombre { get; set; } = null!;
        public string Identidad { get; set; } = null!;
    }
}
