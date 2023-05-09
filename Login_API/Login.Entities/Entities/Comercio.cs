using System;
using System.Collections.Generic;

namespace Login.Entities.Entities;

public partial class Comercio
{
    public int Id { get; set; }

    public string RazonSocial { get; set; } = null!;

    public int Cuit { get; set; }

    public virtual Persona IdNavigation { get; set; } = null!;
}
