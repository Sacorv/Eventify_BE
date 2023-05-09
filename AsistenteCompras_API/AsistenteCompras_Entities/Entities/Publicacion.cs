using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Publicacion
{
    public int Id { get; set; }

    public int IdProducto { get; set; }

    public int IdComercio { get; set; }

    public decimal Precio { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<ListadoPublicacion> ListadoPublicacions { get; set; } = new List<ListadoPublicacion>();
}
