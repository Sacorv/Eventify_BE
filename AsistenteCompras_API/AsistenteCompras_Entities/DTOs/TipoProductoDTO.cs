namespace AsistenteCompras_Entities.DTOs;

public class TipoProductoDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Ingrediente { get; set; } = true;

    public int Peso { get; set; } = 0;

    public int Unidades { get; set; } = 0;
}
