using System;
using System.Collections.Generic;

namespace Productos_Entities.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string Imagen { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdCategoria { get; set; }

    public string CodigoBarras { get; set; } = null!;

    public virtual DetalleProducto? DetalleProducto { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
}
