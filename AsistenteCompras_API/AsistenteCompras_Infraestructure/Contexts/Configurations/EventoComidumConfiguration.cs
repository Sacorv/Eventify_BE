using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class EventoComidumConfiguration : IEntityTypeConfiguration<EventoComidum>
    {
        public void Configure(EntityTypeBuilder<EventoComidum> builder)
        {
            builder.ToTable("EventoComida");

            builder.HasOne(d => d.IdComidaNavigation).WithMany()
                .HasForeignKey(d => d.IdComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoComida_Comida");

            builder.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoComida_Evento");
        }
    }
}
