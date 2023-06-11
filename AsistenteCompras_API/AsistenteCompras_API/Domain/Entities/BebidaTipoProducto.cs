namespace AsistenteCompras_API.Domain.Entities;

public partial class BebidaTipoProducto
{
    public int Id { get; set; }
    public int IdBebida { get; set; }

    public int IdTipoProducto { get; set; }

    public int Beben { get; set; }

    public int Mililitros { get; set; } = 0;

    public int Unidades { get; set; } = 0;

    public virtual Bebidum IdBebidaNavigation { get; set; } = null!;

    public virtual TipoProducto IdTipoProductoNavigation { get; set; } = null!;
}
