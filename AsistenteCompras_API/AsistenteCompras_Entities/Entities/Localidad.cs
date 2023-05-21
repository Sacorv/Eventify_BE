namespace AsistenteCompras_Entities.Entities;

public partial class Localidad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Comercio> Comercios { get; set; } = new List<Comercio>();
}
