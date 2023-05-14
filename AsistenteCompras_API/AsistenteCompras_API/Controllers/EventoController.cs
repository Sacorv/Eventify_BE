using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Services;
using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        public IEventoService _servicio;

        public EventoController(IEventoService servicio)
        {
            this._servicio = servicio;
        }

        [HttpGet("eventos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PublicacionDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public List<Evento> ObtenerEventos()
        {
            return _servicio.ObtenerEventos();
        }

        //public Evento BuscarEventoPorId()
        //{
        //    return null;
        //}

        [HttpGet("eventosPorLocalidad/{idEvento}/{localidad}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PublicacionDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type =typeof(bool))]
        public IActionResult FiltrarOfertasParaEventoPorLocalidad(int idEvento, String localidad)
        {
            try
            {
                List<PublicacionDTO> ofertasParaEvento = this._servicio.BuscarOfertasPorLocalidadYEvento(idEvento, localidad);
                if(ofertasParaEvento.Count() != 0)
                {
                    return Ok(ofertasParaEvento);
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //public List<Evento> FiltrarEventosPorPrecio()
        //{
        //    return null;
        //}


    }
}
