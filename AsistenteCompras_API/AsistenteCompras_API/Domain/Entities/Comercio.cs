namespace AsistenteCompras_API.Domain.Entities;

public partial class Comercio
{
    public int Id { get; set; }

    public string CUIT { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public string Email { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public int IdLocalidad { get; set; }

    public string Imagen { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Localidad IdLocalidadNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();
}
