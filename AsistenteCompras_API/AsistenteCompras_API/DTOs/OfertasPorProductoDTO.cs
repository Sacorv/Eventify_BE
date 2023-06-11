namespace AsistenteCompras_API.DTOs;
public class OfertasPorProductoDTO
{
    public string NombreProducto { get; set; } = string.Empty;
    public List<OfertaCantidadDTO> Ofertas { get; set; } = new List<OfertaCantidadDTO>();
}
