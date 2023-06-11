namespace AsistenteCompras_API.Domain.Entities;
public partial class Publicacion
{
    public int Id { get; set; }

    public int IdProducto { get; set; }

    public int IdComercio { get; set; }

    public decimal Precio { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaFin { get; set; }
    public virtual Comercio IdComercioNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
