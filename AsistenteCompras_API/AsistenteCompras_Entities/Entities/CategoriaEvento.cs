using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class CategoriaEvento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
