using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class BebidaTipoProductoConfiguration : IEntityTypeConfiguration<BebidaTipoProducto>
{
    public void Configure(EntityTypeBuilder<BebidaTipoProducto> builder)
    {
        builder.ToTable("BebidaTipoProducto");

        builder.HasOne(d => d.IdBebidaNavigation).WithMany()
            .HasForeignKey(d => d.IdBebida)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_BebidaTipoProducto_Bebida");

        builder.HasOne(d => d.IdTipoProductoNavigation).WithMany()
            .HasForeignKey(d => d.IdTipoProducto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_BebidaTipoProducto_TipoProducto");
    }
}
