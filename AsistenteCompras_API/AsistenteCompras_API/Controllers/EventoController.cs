using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Services;
using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers
{
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult ObtenerEventos()
        {
            return Ok(_service.ObtenerEventos());
        }

        //public Evento BuscarEventoPorId()
        //{
        //    return null;
        //}




    }
}
