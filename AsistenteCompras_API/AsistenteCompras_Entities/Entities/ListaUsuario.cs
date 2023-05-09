using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class ListaUsuario
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdEvento { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<ListadoPublicacion> ListadoPublicacions { get; set; } = new List<ListadoPublicacion>();
}
