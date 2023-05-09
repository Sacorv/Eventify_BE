using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Menu
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Rol> IdRols { get; set; } = new List<Rol>();
}
