using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Comercio
{
    public int Id { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Localidad { get; set; } = null!;

    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();
}
