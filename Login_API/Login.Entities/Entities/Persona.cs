using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Persona
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public bool Estado { get; set; }

    public int IdUbicacion { get; set; }

    public int IdRol { get; set; }

    public virtual Comercio? Comercio { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
