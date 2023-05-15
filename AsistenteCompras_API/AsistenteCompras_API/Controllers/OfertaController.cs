using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfertaController : ControllerBase
{

    private IOfertaService _service;

    public OfertaController(IOfertaService service)
    {
        _service = service;
    }

    [HttpGet("ofertasPorLocalidad/{idEvento}/{localidad}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult FiltrarOfertasParaEventoPorLocalidad(int idEvento, String localidad)
    {
        try
        {
            List<OfertaDTO> ofertasParaEvento = _service.BuscarOfertasPorLocalidadYEvento(idEvento, localidad);
            if (ofertasParaEvento.Count != 0)
            {
                return Ok(ofertasParaEvento);
            }
            else
            {
                return NotFound();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("ofertasMasEconomicas/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerOfertasMasEconomicas(int idEvento)
    {
        try
        {
            List<OfertaDTO> ofertasParaEvento = _service.ObtenerListaProductosEconomicosPorEvento(idEvento);
            if (ofertasParaEvento.Count != 0)
            {
                return Ok(ofertasParaEvento);
            }
            else
            {
                return NotFound();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
