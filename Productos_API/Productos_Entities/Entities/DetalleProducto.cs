using System;
using System.Collections.Generic;

namespace Productos_Entities.Entities;

public partial class DetalleProducto
{
    public int Id { get; set; }

    public bool Diabetes { get; set; }

    public bool Vegano { get; set; }

    public bool Vegetariano { get; set; }

    public bool Celiaco { get; set; }

    public int Calorias { get; set; }

    public decimal Peso { get; set; }

    public virtual Producto IdNavigation { get; set; } = null!;
}
