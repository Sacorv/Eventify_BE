using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Service;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfertaController : ControllerBase
{

    private IOfertaService _service;

    private IOfertaServicio _servicio;

    public OfertaController(IOfertaService service, IOfertaServicio servicio)
    {
        _service = service;
        _servicio = servicio;
    }

    [HttpGet("ofertasPorLocalidad/{idLocalidad}/{idComida}/{idBebida}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult FiltrarOfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida)
    {
        try
        {
            List<OfertasDTO> ofertasParaEvento = _servicio.ObtenerOfertasMenorPrecioPorLocalidadPreferida(idLocalidad, idComida, idBebida);
            if (ofertasParaEvento.Count != 0)
            {
                return Ok(ofertasParaEvento);
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

    [HttpPost("ofertasMasEconomicas/{idComida}/{idBebida}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerOfertasMasEconomicas(int idComida, List<int> idLocalidades, int idBebida)
    {
        try
        {
            List<OfertaDTO> ofertasParaEvento = _service.ObtenerListaProductosEconomicosPorEvento(idComida,idLocalidades,idBebida);
            if (ofertasParaEvento.Count != 0)
            {
                return Ok(ofertasParaEvento);
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
