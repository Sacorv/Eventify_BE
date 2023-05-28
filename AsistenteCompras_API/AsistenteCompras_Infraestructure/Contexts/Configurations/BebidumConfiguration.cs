using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class BebidumConfiguration : IEntityTypeConfiguration<Bebidum>
    {
        public void Configure(EntityTypeBuilder<Bebidum> builder)
        {
            builder.Property(e => e.TipoBebida).HasMaxLength(50);
        }
    }
}
