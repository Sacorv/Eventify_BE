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

    public virtual DbSet<BebidaTipoProducto> BebidaTipoProductos { get; set; }

    public virtual DbSet<Bebidum> Bebida { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Comercio> Comercios { get; set; }

    public virtual DbSet<ComidaTipoProducto> ComidaTipoProductos { get; set; }

    public virtual DbSet<Comidum> Comida { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<EventoBebidum> EventoBebida { get; set; }

    public virtual DbSet<EventoComidum> EventoComida { get; set; }

    public virtual DbSet<Localidad> Localidads { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Publicacion> Publicacions { get; set; }

    public virtual DbSet<TipoProducto> TipoProductos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SCARLET-PC;Database=AsistenteCompras;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BebidaTipoProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BebidaTipoProducto");

            entity.HasOne(d => d.IdBebidaNavigation).WithMany()
                .HasForeignKey(d => d.IdBebida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BebidaTipoProducto_Bebida");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BebidaTipoProducto_TipoProducto");
        });

        modelBuilder.Entity<Bebidum>(entity =>
        {
            entity.Property(e => e.TipoBebida).HasMaxLength(50);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Comercio>(entity =>
        {
            entity.ToTable("Comercio");

            entity.Property(e => e.Direccion).HasMaxLength(50);
            entity.Property(e => e.Latitud).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.RazonSocial).HasMaxLength(50);

            entity.HasOne(d => d.IdLocalidadNavigation).WithMany(p => p.Comercios)
                .HasForeignKey(d => d.IdLocalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comercio_Localidad");
        });

        modelBuilder.Entity<ComidaTipoProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ComidaTipoProducto");

            entity.HasOne(d => d.IdComidaNavigation).WithMany()
                .HasForeignKey(d => d.IdComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComidaTipoProducto_Comida");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComidaTipoProducto_TipoProducto");
        });

        modelBuilder.Entity<Comidum>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Evento");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<EventoBebidum>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.IdBebidaNavigation).WithMany()
                .HasForeignKey(d => d.IdBebida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoBebida_Bebida");

            entity.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoBebida_Evento");
        });

        modelBuilder.Entity<EventoComidum>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.IdComidaNavigation).WithMany()
                .HasForeignKey(d => d.IdComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoComida_Comida");

            entity.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventoComida_Evento");
        });

        modelBuilder.Entity<Localidad>(entity =>
        {
            entity.ToTable("Localidad");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.Property(e => e.Imagen)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_TipoProducto");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.ToTable("Publicacion");

            entity.Property(e => e.Precio).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.IdComercioNavigation).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.IdComercio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicacion_Comercio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicacion_Producto");
        });

        modelBuilder.Entity<TipoProducto>(entity =>
        {
            entity.ToTable("TipoProducto");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
