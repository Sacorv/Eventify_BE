using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfertaController : ControllerBase
{
    private readonly IOfertaService _service;

    public OfertaController(IOfertaService service)
    {
        _service = service;
    }

    [HttpGet("ofertasPorLocalidad/{idLocalidad}/{idComida}/{idBebida}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult FiltrarOfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida)
    {
        try
        {
            List<OfertaDTO> ofertasParaEvento = _service.ObtenerOfertasMenorPrecioPorLocalidadPreferida(idLocalidad, idComida, idBebida);
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
            List<OfertaDTO> ofertasParaEvento = _service.ObtenerListaProductosEconomicosPorEvento(idComida, idLocalidades, idBebida);
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

    [HttpGet("ofertasMenorPrecioPorZona/{latitud}/{longitud}/{distancia}/{idComida}/{idBebida}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult OfertasMenorPrecioDentroDelRadio(double latitud, double longitud, float distancia, int idComida, int idBebida)
    {
        try
        {
            List<OfertaDTO> comercios = _service.ObtenerOfertasPorZonaGeografica(latitud, longitud, distancia, idComida, idBebida);
            if (comercios.Count != 0)
            {
                return Ok(comercios);
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
