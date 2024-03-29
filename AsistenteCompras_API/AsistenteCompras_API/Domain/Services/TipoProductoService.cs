﻿using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public class TipoProductoService : ITipoProductoService
{
    private ITipoProductoRepository _tipoProductoRepository;

    public TipoProductoService(ITipoProductoRepository tipoProductoRepository)
    {
        _tipoProductoRepository = tipoProductoRepository;
    }

    public List<int> ObtenerIdTipoProductosBebida(int idBebida)
    {
        return _tipoProductoRepository.ObtenerIdTipoProductosBebida(idBebida);
    }

    public List<int> ObtenerIdTipoProductosComida(int idComida)
    {
        return _tipoProductoRepository.ObtenerIdTipoProductosComida(idComida);
    }

    public List<int> ObtenerTiposDeBebida(List<int> idBebida)
    {
        return _tipoProductoRepository.ObtenerIdTipoProductosBebidaV2(idBebida);
    }

    public List<int> ObtenerTiposDeComida(List<int> idComida)
    {
        return _tipoProductoRepository.ObtenerIdTipoProductosComidaV2(idComida);
    }

    public List<TipoProductoDTO> ObtenerTodosLosTiposDeProductos()
    {
        return _tipoProductoRepository.ObtenerTodosLosTiposDeProductos();
    }
}
