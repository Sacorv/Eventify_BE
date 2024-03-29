﻿namespace AsistenteCompras_API.Domain.Entities;

public partial class Evento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<ListadoDeOfertas> ListadoDeOfertas { get; set; } = new List<ListadoDeOfertas>();
}
