using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_Entities.Entities;

public partial class AsistenteContext : DbContext
{
    public AsistenteContext()
    {
    }

    public AsistenteContext(DbContextOptions<AsistenteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaEvento> CategoriaEventos { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<ListaUsuario> ListaUsuarios { get; set; }

    public virtual DbSet<ListadoPublicacion> ListadoPublicacions { get; set; }

    public virtual DbSet<Publicacion> Publicacions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SCARLET-PC;Database=Asistente;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaEvento>(entity =>
        {
            entity.ToTable("CategoriaEvento");

            entity.Property(e => e.FechaCreacion).HasColumnType("date");
            entity.Property(e => e.FechaModificacion).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Evento");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaEventoNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCategoriaEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evento_CategoriaEvento");
        });

        modelBuilder.Entity<ListaUsuario>(entity =>
        {
            entity.ToTable("ListaUsuario");

            entity.Property(e => e.FechaCreacion).HasColumnType("date");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.ListaUsuarios)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListaUsuario_Evento");
        });

        modelBuilder.Entity<ListadoPublicacion>(entity =>
        {
            entity.HasKey(e => new { e.IdListaUsuario, e.IdPublicacion });

            entity.ToTable("ListadoPublicacion");

            entity.HasOne(d => d.IdListaUsuarioNavigation).WithMany(p => p.ListadoPublicacions)
                .HasForeignKey(d => d.IdListaUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoPublicacion_ListaUsuario");

            entity.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.ListadoPublicacions)
                .HasForeignKey(d => d.IdPublicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListadoPublicacion_Publicacion");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.ToTable("Publicacion");

            entity.Property(e => e.FechaCreacion).HasColumnType("date");
            entity.Property(e => e.FechaModificacion).HasColumnType("date");
            entity.Property(e => e.Precio).HasColumnType("decimal(4, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
