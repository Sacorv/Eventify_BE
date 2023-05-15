using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_Entities.Entities;

public partial class AsistenteComprasContext : DbContext
{
    public AsistenteComprasContext()
    {
    }

    public AsistenteComprasContext(DbContextOptions<AsistenteComprasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Comercio> Comercios { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<EventoProducto> EventoProductos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Publicacion> Publicacions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FPKLG0I\\SQLEXPRESS;Database=AsistenteCompras;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Comercio>(entity =>
        {
            entity.ToTable("Comercio");

            entity.Property(e => e.Direccion).HasMaxLength(50);
            entity.Property(e => e.Localidad).HasMaxLength(50);
            entity.Property(e => e.RazonSocial).HasMaxLength(50);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Evento");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<EventoProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EventoProducto");

            entity.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoProducto_Evento");

            entity.HasOne(d => d.IdProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoProducto_Producto");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.ToTable("Publicacion");

            entity.Property(e => e.Precio).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.IdComercioNavigation).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.IdComercio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicacion_Comercio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicacion_Producto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
