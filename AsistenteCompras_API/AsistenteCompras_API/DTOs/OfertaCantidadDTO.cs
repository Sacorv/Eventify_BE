namespace AsistenteCompras_API.DTOs;

public class OfertaCantidadDTO
{
    public double Cantidad { get; set; }
    public OfertaDTO Oferta { get; set; } = new OfertaDTO();

    public double Subtotal { get; set; }
}
