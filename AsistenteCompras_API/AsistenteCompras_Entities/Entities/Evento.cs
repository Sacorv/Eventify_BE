using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class Evento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdCategoriaEvento { get; set; }

    public virtual CategoriaEvento IdCategoriaEventoNavigation { get; set; } = null!;

    public virtual ICollection<ListaUsuario> ListaUsuarios { get; set; } = new List<ListaUsuario>();
}
