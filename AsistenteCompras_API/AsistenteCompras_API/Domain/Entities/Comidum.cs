namespace AsistenteCompras_API.Domain.Entities;

public partial class Comidum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Comensales { get; set; }
}
