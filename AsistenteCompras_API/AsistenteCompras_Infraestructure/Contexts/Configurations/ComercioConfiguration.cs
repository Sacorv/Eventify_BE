﻿using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_Infraestructure.Contexts.Configurations
{
    public class ComercioConfiguration : IEntityTypeConfiguration<Comercio>
    {
        public void Configure(EntityTypeBuilder<Comercio> builder)
        {
            builder.ToTable("Comercio");

            builder.Property(e => e.Direccion).HasMaxLength(50);
            builder.Property(e => e.Latitud).HasColumnType("decimal(18, 6)");
            builder.Property(e => e.Longitud).HasColumnType("decimal(18, 6)");
            builder.Property(e => e.RazonSocial).HasMaxLength(50);

            builder.HasOne(d => d.IdLocalidadNavigation).WithMany(p => p.Comercios)
                .HasForeignKey(d => d.IdLocalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comercio_Localidad");
        }
    }
}
