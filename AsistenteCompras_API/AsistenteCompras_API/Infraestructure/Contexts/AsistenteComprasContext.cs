using AsistenteCompras_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AsistenteCompras_API.Infraestructure.Contexts;

public partial class AsistenteComprasContext : DbContext
{
    private IConfiguration _config;
    public AsistenteComprasContext(IConfiguration config)
    {
        _config = config;
    }

    public AsistenteComprasContext(DbContextOptions<AsistenteComprasContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
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

    public virtual DbSet<ListadoDeOfertas> ListadoDeOfertas { get; set; }

    public virtual DbSet<OfertaElegida> OfertaElegida { get; set; }

    public virtual DbSet<ListadoOfertasBebida> ListadoOfertasBebida { get; set; }

    public virtual DbSet<ListadoOfertasComida> ListadoOfertasComida { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
