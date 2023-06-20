using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations
{
    public class OfertaElegidaConfiguration : IEntityTypeConfiguration<OfertaElegida>
    {
        public void Configure(EntityTypeBuilder<OfertaElegida> builder)
        {
            builder.ToTable("OfertaElegida");

            builder.Property(e => e.NombreProducto)
            .HasMaxLength(50)
            .IsUnicode(false);

            builder.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Precio).HasColumnType("decimal(18, 2)");


            builder.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.OfertasElegidas)
                .HasForeignKey(d => d.IdPublicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoDeOfertas_Publicacion");

            builder.HasOne(d => d.IdListadoDeOfertasNavigation).WithMany(p => p.OfertasElegidas)
                .HasForeignKey(d => d.IdListadoDeOfertas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoDeOfertas_ListadoDeOfertas");

        }
    }
}
