using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class FormaPagoComercioConfiguation : IEntityTypeConfiguration<FormaPagoComercio>
    {
        public void Configure(EntityTypeBuilder<FormaPagoComercio> builder)
        {
            builder.ToTable("FormaPagoComercio");

            builder.HasOne(d => d.IdComercioNavigation).WithMany()
                .HasForeignKey(d => d.IdComercio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormaPagoComercio_Comercio");

            builder.HasOne(d => d.IdFormaPagoNavigation).WithMany()
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormaPagoComercio_FormaPago");
        }
    }
}
