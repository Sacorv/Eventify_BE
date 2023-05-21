using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class ComidaTipoProducto
{
    public int IdComida { get; set; }

    public int IdTipoProducto { get; set; }

    public virtual Comidum IdComidaNavigation { get; set; } = null!;

    public virtual TipoProducto IdTipoProductoNavigation { get; set; } = null!;
}
