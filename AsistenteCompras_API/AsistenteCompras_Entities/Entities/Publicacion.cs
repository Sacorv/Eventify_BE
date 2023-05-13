using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Publicacion
{
    public int Id { get; set; }

    public int IdProducto { get; set; }

    public int IdComercio { get; set; }

    public string Precio { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual Comercio IdComercioNavigation { get; set; } = null!;
}
