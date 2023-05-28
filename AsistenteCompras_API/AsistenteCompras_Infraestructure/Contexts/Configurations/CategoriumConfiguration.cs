using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class CategoriumConfiguration : IEntityTypeConfiguration<Categorium>
    {
        public void Configure(EntityTypeBuilder<Categorium> builder)
        {
            builder.Property(e => e.Nombre).HasMaxLength(50);
        }
    }
}
