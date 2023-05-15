using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        }
    }

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
