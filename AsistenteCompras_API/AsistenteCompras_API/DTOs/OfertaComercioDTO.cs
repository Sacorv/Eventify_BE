namespace AsistenteCompras_API.DTOs;

public class OfertaComercioDTO
{
    public String Nombre { get; set; } = null!;

    public String Imagen { get; set; } = null!;

    public DateTime FechaFin { get; set; }

    public Decimal Precio { get; set; }
}
