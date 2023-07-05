namespace AsistenteCompras_API.DTOs;

public class OfertaAPublicarDTO
{
    public int IdComercio { get; set; }
    public int IdProducto { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaFin { get; set; }
}
