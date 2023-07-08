namespace AsistenteCompras_API.Domain;
public class OfertasPorProducto
{
    public string NombreProducto { get; set; } = string.Empty;
    public List<OfertaCantidad> Ofertas { get; set; } = new List<OfertaCantidad>();
}
