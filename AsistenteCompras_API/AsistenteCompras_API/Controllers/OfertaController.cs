using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AsistenteCompras_API.Domain;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfertaController : ControllerBase
{
    private readonly IOfertaService _ofertaService;

    public OfertaController(IOfertaService ofertaService)
    {
        _ofertaService = ofertaService;
    }


    [HttpPost("listadoOfertas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasPorProducto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerListadoOfertasMasEconomicas([FromBody] Filtro filtro)
    {
        try
        {
            List<OfertasPorProducto> ofertas = _ofertaService.GenerarListaDeOfertas(filtro);
            if (ofertas.Count != 0)
            {
                return Ok(ofertas);
            }
            else
            {
                return NoContent();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("recorrerMenos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasPorComercioDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerOfertasEnMenorRecorrido([FromBody] Filtro filtro)
    {
        try
        {
            List<OfertasPorComercioDTO> ofertas = _ofertaService.ListaRecorrerMenos(filtro);
            if (ofertas.Count != 0)
            {
                return Ok(ofertas);
            }
            else
            {
                return NoContent();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }
}
