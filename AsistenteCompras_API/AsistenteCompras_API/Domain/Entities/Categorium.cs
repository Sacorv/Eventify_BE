﻿namespace AsistenteCompras_API.Domain.Entities;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
