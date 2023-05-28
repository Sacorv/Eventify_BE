using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class LocalidadConfiguration : IEntityTypeConfiguration<Localidad>
    {
        public void Configure(EntityTypeBuilder<Localidad> builder)
        {
            builder.ToTable("Localidad");

            builder.Property(e => e.Nombre).HasMaxLength(50);
        }
    }
}
