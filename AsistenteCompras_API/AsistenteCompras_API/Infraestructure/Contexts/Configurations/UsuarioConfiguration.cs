using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.Property(e => e.Nombre).HasMaxLength(50);
            builder.Property(e => e.Apellido).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(50);
            builder.Property(e => e.Clave).HasMaxLength(50);
        }
    }
}
