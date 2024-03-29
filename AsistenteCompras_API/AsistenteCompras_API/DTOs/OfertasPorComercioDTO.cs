﻿using AsistenteCompras_API.Domain;

namespace AsistenteCompras_API.DTOs;

public class OfertasPorComercioDTO
{
    public string NombreComercio { get; set; } = String.Empty;

    public string ImagenComercio { get; set; } = String.Empty;

    public double Distancia { get; set; }  

    public List<OfertaCantidad> Ofertas { get; set; } = new List<OfertaCantidad>();
}
