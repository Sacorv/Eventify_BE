using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventoController : ControllerBase
{

    private IEventoService _eventoService;

    public EventoController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }
    
    [HttpGet("eventos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evento>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerEventos()
    {
        return Ok(_eventoService.ObtenerEventos());
    }


    [HttpGet("comidas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(List<Comidum>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerComidasPorEvento(int idEvento)
    {
        return Ok(_eventoService.ObtenerComidas(idEvento));
    }


    [HttpGet("bebidas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Bebidum>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerBebidasDelEvento(int idEvento)
    {
        return Ok(_eventoService.ObtenerBebidas(idEvento));
    }


    [HttpGet("listado")]
    [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<TipoProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerListaParaEvento(int idEvento, int idComida, int idBebida)
    {

        return Ok(_eventoService.ObtenerListadoParaEvento(idEvento, idComida, idBebida));
    }

    [HttpPost("listadoConCantidades/{invitados}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ListadoCantidades(ProductosABuscarDTO productosABuscar,int invitados)
    {
        try
        {
            List<TipoProductoDTO> listaCompras = _eventoService.ObtenerListadoParaEvento(productosABuscar,invitados);
            if (listaCompras.Count != 0)
            {
                return Ok(listaCompras);
            }
            else
            {
                return NoContent();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
