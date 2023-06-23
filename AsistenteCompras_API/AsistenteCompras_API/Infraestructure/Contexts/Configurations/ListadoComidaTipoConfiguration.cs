using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations
{
    public class ListadoOfertasComidaConfiguration : IEntityTypeConfiguration<ListadoOfertasComida>
    {
        public void Configure(EntityTypeBuilder<ListadoOfertasComida> builder)
        {
            builder.ToTable("ListadoOfertasComida");

            builder.HasOne(d => d.IdComidaNavigation).WithMany()
                .HasForeignKey(d => d.IdComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoOfertasComida_Comida");

            builder.HasOne(d => d.IdListadoDeOfertasNavigation).WithMany()
                .HasForeignKey(d => d.IdListadoDeOfertas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoOfertasComida_ListadoDeOfertas");
        }
    }
}
