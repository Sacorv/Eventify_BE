namespace AsistenteCompras_Entities.Entities;

public partial class BebidaTipoProducto
{
    public int IdBebida { get; set; }

    public int IdTipoProducto { get; set; }

    public virtual Bebidum IdBebidaNavigation { get; set; } = null!;

    public virtual TipoProducto IdTipoProductoNavigation { get; set; } = null!;
}
