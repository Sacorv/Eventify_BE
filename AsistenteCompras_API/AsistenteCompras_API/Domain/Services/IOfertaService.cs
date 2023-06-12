﻿using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IOfertaService
{
    //List<OfertaDTO> ObtenerOfertasEconomicasPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida);
    
    List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);

    List<OfertaCantidadDTO> GenerarListaPersonalizada(FiltroDTO filtro);

    List<OfertasPorProductoDTO> GenerarListaDeOfertas(FiltroDTO filtro);

}