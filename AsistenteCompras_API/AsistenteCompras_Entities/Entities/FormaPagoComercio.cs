namespace AsistenteCompras_Entities.Entities;

public partial class FormaPagoComercio
{
    public int Id { get; set; }
    public int IdComercio { get; set; }

    public int IdFormaPago { get; set; }

    public bool Estado { get; set; }

    public virtual Comercio IdComercioNavigation { get; set; } = null!;

    public virtual FormaPago IdFormaPagoNavigation { get; set; } = null!;
}
