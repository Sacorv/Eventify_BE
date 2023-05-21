﻿using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventoController : ControllerBase
{

    private IEventoService _service;

    public EventoController(IEventoService service)
    {
        _service = service;
    }
    
    [HttpGet("eventos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evento>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerEventos()
    {
        return Ok(_service.ObtenerEventos());
    }

    //public Evento BuscarEventoPorId()
    //{
    //    return null;
    //}

    [HttpGet("comidas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(List<Comidum>))]
    public IActionResult ObtenerComidasPorEvento(int idEvento)
    {
        return Ok(_service.ObtenerComidas(idEvento));
    }

    [HttpGet("bebidas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evento>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerBebidasDelEvento(int idEvento)
    {
        return Ok(_service.ObtenerBebidasPosibles(idEvento));
    }
}
