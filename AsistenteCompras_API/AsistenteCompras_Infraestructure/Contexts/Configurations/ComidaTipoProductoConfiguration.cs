using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class ComidaTipoProductoConfiguration : IEntityTypeConfiguration<ComidaTipoProducto>
    {
        public void Configure(EntityTypeBuilder<ComidaTipoProducto> builder)
        {
            builder.ToTable("ComidaTipoProducto");

            builder.HasOne(d => d.IdComidaNavigation).WithMany()
                .HasForeignKey(d => d.IdComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComidaTipoProducto_Comida");

            builder.HasOne(d => d.IdTipoProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComidaTipoProducto_TipoProducto");
        }
    }
}
