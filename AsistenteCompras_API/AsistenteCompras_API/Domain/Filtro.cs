namespace AsistenteCompras_API.Domain;

public class Filtro
{
    public double LatitudUbicacion { get; set; }
    public double LongitudUbicacion { get; set; }
    public float Distancia { get; set; }
    public List<int> Comidas { get; set; } = null!;
    public List<int> Bebidas { get; set; } = null!;
    public List<string>? MarcasComida { get; set; } = null!;
    public List<string>? MarcasBebida { get; set; } = null!;
    public Dictionary<string, double>? CantidadProductos { get; set; } = null!;

}
