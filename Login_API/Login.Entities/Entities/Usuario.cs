using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public virtual Persona IdNavigation { get; set; } = null!;
}
