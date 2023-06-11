using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class PublicacionConfiguration : IEntityTypeConfiguration<Publicacion>
{
    public void Configure(EntityTypeBuilder<Publicacion> builder)
    {
        builder.ToTable("Publicacion");

        builder.Property(e => e.FechaFin).HasColumnType("date");
        builder.Property(e => e.Precio).HasColumnType("decimal(7, 2)");

        builder.HasOne(d => d.IdComercioNavigation).WithMany(p => p.Publicacions)
            .HasForeignKey(d => d.IdComercio)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Publicacion_Comercio");

        builder.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Publicacions)
            .HasForeignKey(d => d.IdProducto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Publicacion_Producto");
    }
}
