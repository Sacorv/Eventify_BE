using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Ubicacion
{
    public int Id { get; set; }

    public string Calle { get; set; } = null!;

    public int Altura { get; set; }

    public int Departamento { get; set; }

    public int Latitud { get; set; }

    public int Longitud { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
