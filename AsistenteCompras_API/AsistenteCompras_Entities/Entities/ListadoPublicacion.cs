using System;
using System.Collections.Generic;

namespace AsistenteCompras_Entities.Entities;

public partial class ListadoPublicacion
{
    public int IdListaUsuario { get; set; }

    public int IdPublicacion { get; set; }

    public int Unidades { get; set; }

    public int Total { get; set; }

    public bool Apto { get; set; }

    public virtual ListaUsuario IdListaUsuarioNavigation { get; set; } = null!;

    public virtual Publicacion IdPublicacionNavigation { get; set; } = null!;
}
