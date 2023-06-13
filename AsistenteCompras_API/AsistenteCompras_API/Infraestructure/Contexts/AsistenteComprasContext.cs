using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AsistenteCompras_API.Infraestructure.Contexts;

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
    public virtual DbSet<Usuario> Usuarios { get; set; }

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
        modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
