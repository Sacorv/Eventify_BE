using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Login.Entities.Entities;

public partial class AsistenteContext : DbContext
{
    public AsistenteContext()
    {
    }

    public AsistenteContext(DbContextOptions<AsistenteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comercio> Comercios { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SCARLET-PC;Database=Asistente;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comercio>(entity =>
        {
            entity.ToTable("Comercio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Comercio)
                .HasForeignKey<Comercio>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comercio_Persona");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.FechaCreacion).HasColumnType("date");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdMenus)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuRol",
                    r => r.HasOne<Rol>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuRol_Rol"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("IdMenu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuRol_Menu"),
                    j =>
                    {
                        j.HasKey("IdMenu", "IdRol");
                        j.ToTable("MenuRol");
                    });
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("Persona");

            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("date");
            entity.Property(e => e.FechaModificacion).HasColumnType("date");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Rol");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Ubicacion");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");

            entity.Property(e => e.FechaCreacion).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.ToTable("Ubicacion");

            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Persona");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
