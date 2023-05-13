using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Evento> IdEventos { get; set; } = new List<Evento>();
}
