using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class EventoProducto
{
    public int IdEvento { get; set; }

    public int IdProducto { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
