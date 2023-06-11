using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class FormaPagoConfiguration : IEntityTypeConfiguration<FormaPago>
{
    public void Configure(EntityTypeBuilder<FormaPago> builder)
    {
        builder.ToTable("FormaPago");

        builder.Property(e => e.Nombre)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}
