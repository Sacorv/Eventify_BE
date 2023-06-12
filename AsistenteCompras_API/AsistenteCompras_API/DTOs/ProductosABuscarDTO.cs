namespace AsistenteCompras_API.DTOs;

public class ProductosABuscarDTO
{
    public List<int> IdComidas { get; set; } = new List<int>();

    public List<int> IdBebidas { get; set; } = new List<int>();
}
