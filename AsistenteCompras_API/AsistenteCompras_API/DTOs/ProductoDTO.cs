namespace AsistenteCompras_API.DTOs;

public class ProductoDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public int IdTipoProducto { get; set; }

    public string Imagen { get; set; } = null!;
}
