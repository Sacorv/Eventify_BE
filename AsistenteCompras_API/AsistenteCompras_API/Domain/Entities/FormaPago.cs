﻿namespace AsistenteCompras_API.Domain.Entities;

public partial class FormaPago
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }
}
