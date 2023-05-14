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

        public EventoService _servicio;

        public EventoController(EventoService servicio)
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
        public List<PublicacionDTO> FiltrarOfertasParaEventoPorLocalidad(int idEvento, String localidad)
        {
            return this._servicio.BuscarOfertasPorLocalidadYEvento(idEvento, localidad);
        }

        //public List<Evento> FiltrarEventosPorPrecio()
        //{
        //    return null;
        //}


    }
}
