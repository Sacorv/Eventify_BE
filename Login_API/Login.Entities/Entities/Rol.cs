using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();

    public virtual ICollection<Menu> IdMenus { get; set; } = new List<Menu>();
}
