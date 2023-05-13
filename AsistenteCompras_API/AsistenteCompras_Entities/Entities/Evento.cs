﻿using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Evento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Producto> IdProductos { get; set; } = new List<Producto>();
}