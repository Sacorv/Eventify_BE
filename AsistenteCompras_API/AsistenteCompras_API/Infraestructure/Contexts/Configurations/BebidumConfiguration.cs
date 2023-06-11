using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class BebidumConfiguration : IEntityTypeConfiguration<Bebidum>
{
    public void Configure(EntityTypeBuilder<Bebidum> builder)
    {
        builder.Property(e => e.TipoBebida).HasMaxLength(50);
    }
}
