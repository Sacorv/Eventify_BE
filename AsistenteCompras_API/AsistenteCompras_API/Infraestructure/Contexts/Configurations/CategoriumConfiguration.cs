using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class CategoriumConfiguration : IEntityTypeConfiguration<Categorium>
{
    public void Configure(EntityTypeBuilder<Categorium> builder)
    {
        builder.Property(e => e.Nombre).HasMaxLength(50);
    }
}
