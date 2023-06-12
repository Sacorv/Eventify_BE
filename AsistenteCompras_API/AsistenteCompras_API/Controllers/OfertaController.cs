using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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


    [HttpPost("listaPersonalizada")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaCantidadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerOfertasMasEconomicasSegunFiltros([FromBody]FiltroDTO filtro)
    {
        try
        {
            List<OfertaCantidadDTO> ofertas = _ofertaService.GenerarListaPersonalizada(filtro);
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


    [HttpPost("listadoOfertas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasPorProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerListadoOfertasMasEconomicas([FromBody] FiltroDTO filtro)
    {
        try
        {
            List<OfertasPorProductoDTO> ofertas = _ofertaService.GenerarListaDeOfertas(filtro);
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
    public IActionResult ObtenerOfertasEnMenorRecorrido([FromBody] FiltroDTO filtro)
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





    //[HttpPost("ofertasMasEconomicas/{idComida}/{idBebida}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    //public IActionResult ObtenerOfertasMasEconomicas(int idComida, List<int> idLocalidades, int idBebida)
    //{
    //    try
    //    {
    //        List<OfertaDTO> ofertasParaEvento = _ofertaService.ObtenerListaProductosEconomicosPorEvento(idComida, idLocalidades, idBebida);
    //        if (ofertasParaEvento.Count != 0)
    //        {
    //            return Ok(ofertasParaEvento);
    //        }
    //        else
    //        {
    //            return NoContent();
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        return BadRequest(e.Message);
    //    }
    //}

    //[HttpGet("ofertasPorLocalidad/{idLocalidad}/{idComida}/{idBebida}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
    //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    //public IActionResult ObtenerOfertasMenorRecorrido(int idLocalidad, int idComida, int idBebida)
    //{
    //    try
    //    {
    //        List<OfertaDTO> ofertasParaEvento = _ofertaService.ObtenerOfertasEconomicasPorLocalidadPreferida(idLocalidad, idComida, idBebida);
    //        if (ofertasParaEvento.Count != 0)
    //        {
    //            return Ok(ofertasParaEvento);
    //        }
    //        else
    //        {
    //            return NoContent();
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        return BadRequest(e.Message);
    //    }
    //}


}
