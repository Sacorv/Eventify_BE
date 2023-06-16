using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations
{
    public class ListadoDeOfertasConfiguration : IEntityTypeConfiguration<ListadoDeOfertas>
    {
        public void Configure(EntityTypeBuilder<ListadoDeOfertas> builder)
        {
            builder.ToTable("ListadoDeOfertas");

            builder.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ListadoDeOfertas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoDeOfertas_Usuario");
        }
    }
}
