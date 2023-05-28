using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class BebidaTipoProductoConfiguration : IEntityTypeConfiguration<BebidaTipoProducto>
    {
        public void Configure(EntityTypeBuilder<BebidaTipoProducto> builder)
        {
            builder.HasNoKey()
                  .ToTable("BebidaTipoProducto");

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
}
