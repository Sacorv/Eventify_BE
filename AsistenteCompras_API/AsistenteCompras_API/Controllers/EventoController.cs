using AsistenteCompras_Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        public List<Evento> ObtenerEventos()
        {
            return null;
        }

        public Evento BuscarEventoPorId()
        {
            return null;
        }

        public List<Evento> FiltrarEventosPorLocalidad()
        {
            return null;
        }

        public List<Evento> FiltrarEventosPorPrecio()
        {
            return null;
        }


    }
}
