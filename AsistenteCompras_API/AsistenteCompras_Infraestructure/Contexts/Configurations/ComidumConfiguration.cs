using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class ComidumConfiguration : IEntityTypeConfiguration<Comidum>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Comidum> builder)
        {
            builder.Property(e => e.Nombre).HasMaxLength(50);
        }
    }
}
