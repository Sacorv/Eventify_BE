using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class TipoProductoConfiguration : IEntityTypeConfiguration<TipoProducto>
    {
        public void Configure(EntityTypeBuilder<TipoProducto> builder)
        {
            builder.ToTable("TipoProducto");

            builder.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
