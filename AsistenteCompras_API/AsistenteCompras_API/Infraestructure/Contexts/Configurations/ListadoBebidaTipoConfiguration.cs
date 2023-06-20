using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations
{
    public class ListadoOfertasBebidaConfiguration : IEntityTypeConfiguration<ListadoOfertasBebida>
    {
        public void Configure(EntityTypeBuilder<ListadoOfertasBebida> builder)
        {
            builder.ToTable("ListadoOfertasBebida");

            builder.HasOne(d => d.IdBebidaNavigation).WithMany()
                .HasForeignKey(d => d.IdBebida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoOfertasBebida_Bebida");

            builder.HasOne(d => d.IdListadoDeOfertasNavigation).WithMany()
                .HasForeignKey(d => d.IdListadoDeOfertas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoOfertasBebida_ListadoDeOfertas");
        }
    }
}
