namespace AsistenteCompras_API.DTOs;

public class FiltroDTO
{
    public double LatitudUbicacion { get; set; }
    public double LongitudUbicacion { get; set; }
    public float Distancia { get; set; }
    public List<int>? Comidas { get; set; }
    public List<int>? Bebidas { get; set; }
    public List<String>? MarcasComida { get; set; }
    public List<String>? MarcasBebida { get; set; }
    public Dictionary<string, double>? CantidadProductos { get; set; }

}
