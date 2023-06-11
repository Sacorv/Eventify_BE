using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class EventoBebidumConfiguration : IEntityTypeConfiguration<EventoBebidum>
{
    public void Configure(EntityTypeBuilder<EventoBebidum> builder)
    {
        builder.ToTable("EventoBebida");

        builder.HasOne(d => d.IdBebidaNavigation).WithMany()
            .HasForeignKey(d => d.IdBebida)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_EventoBebida_Bebida");

        builder.HasOne(d => d.IdEventoNavigation).WithMany()
            .HasForeignKey(d => d.IdEvento)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_EventoBebida_Evento");
    }
}
