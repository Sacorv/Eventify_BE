namespace AsistenteCompras_API.DTOs;

public class OfertaComercioDTO
{
    public String Nombre { get; set; } = null!;

    public String Imagen { get; set; } = null!;

    public string FechaFin { get; set; } = null!;

    public Decimal Precio { get; set; }
}
