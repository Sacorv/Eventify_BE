using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsistenteCompras_API.Infraestructure.Contexts.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Producto");

        builder.Property(e => e.Imagen)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.Marca)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.Nombre)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.Peso).HasColumnType("decimal(9, 2)");
        builder.Property(e => e.Unidades).HasColumnType("int");

        builder.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
            .HasForeignKey(d => d.IdCategoria)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Producto_Categoria");

        builder.HasOne(d => d.IdTipoProductoNavigation).WithMany(p => p.Productos)
            .HasForeignKey(d => d.IdTipoProducto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Producto_TipoProducto");
    }
}
