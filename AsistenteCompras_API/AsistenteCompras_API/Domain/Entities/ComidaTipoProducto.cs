namespace AsistenteCompras_API.Domain.Entities;

public partial class ComidaTipoProducto
{
    public int Id { get; set; }
    public int IdComida { get; set; }

    public int IdTipoProducto { get; set; }

    public int Unidades { get; set; } = 0;

    public int Gramos { get; set; } = 0;

    public virtual Comidum IdComidaNavigation { get; set; } = null!;

    public virtual TipoProducto IdTipoProductoNavigation { get; set; } = null!;
}
