namespace AsistenteCompras_API.Domain.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdCategoria { get; set; }

    public int IdTipoProducto { get; set; }

    public string Imagen { get; set; } = null!;

    public int Peso { get; set; }

    public int Unidades { get; set; }

    public string CodigoBarras { get; set; } = null!;

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual TipoProducto IdTipoProductoNavigation { get; set; } = null!;

    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();
}
