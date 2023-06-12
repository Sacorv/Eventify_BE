using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class ComidumConfiguration : IEntityTypeConfiguration<Comidum>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Comidum> builder)
    {
        builder.Property(e => e.Nombre).HasMaxLength(50);
    }
}
