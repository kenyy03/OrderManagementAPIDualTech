using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Data.Maps
{
    public class DetalleOrdenMap : IEntityTypeConfiguration<DetalleOrden>
    {
        public void Configure(EntityTypeBuilder<DetalleOrden> builder)
        {
            builder.ToTable("DetalleOrden");

            builder.HasKey(d => d.DetalleOrdenId);

            builder.Property(d => d.DetalleOrdenId)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.OrdenId)
                .IsRequired();

            builder.Property(d => d.ProductoId)
                .IsRequired();

            builder.Property(d => d.Cantidad)
                .IsRequired();

            builder.Property(d => d.Impuesto)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(d => d.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(d => d.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.HasIndex(d => d.OrdenId)
                .HasDatabaseName("IX_DetalleOrden_OrdenId");

            builder.HasIndex(d => d.ProductoId)
                .HasDatabaseName("IX_DetalleOrden_ProductoId");

            builder.HasOne(d => d.Orden)
                .WithMany(o => o.DetallesOrden)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleOrden_Orden");

            builder.HasOne(d => d.Producto)
                .WithMany(p => p.DetallesOrden)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DetalleOrden_Producto");
        }
    }
}
