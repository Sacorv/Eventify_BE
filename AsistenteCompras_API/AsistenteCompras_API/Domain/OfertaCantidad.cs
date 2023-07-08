namespace AsistenteCompras_API.Domain;

public class OfertaCantidad
{
    public double Cantidad { get; set; }
    public Oferta? Oferta { get; set; }

    public double Subtotal { get; set; }
}
