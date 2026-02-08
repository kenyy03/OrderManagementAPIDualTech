using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Data.Maps
{
    public class OrdenMap : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.ToTable("Orden");

            builder.HasKey(o => o.OrdenId);

            builder.Property(o => o.OrdenId)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.ClienteId)
                .IsRequired();

            builder.Property(o => o.Impuesto)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.FechaCreacion)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(o => o.ClienteId)
                .HasDatabaseName("IX_Orden_ClienteId");

            builder.HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orden_Cliente");

            builder.HasMany(o => o.DetallesOrden)
                .WithOne(d => d.Orden)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
