namespace OrderManagementAPI.Models
{
    public class Cliente
    {
        public long ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Identidad { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}
