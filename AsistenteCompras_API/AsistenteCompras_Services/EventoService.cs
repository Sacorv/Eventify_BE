using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services
{
    public class EventoService : IEventoService
    {

        private AsistenteComprasContext _context;

        public EventoService(AsistenteComprasContext context)
        {
            this._context = context;
        }

        public List<Evento> ObtenerEventos()
        {
            return this._context.Eventos.ToList();
        }

        

       
    }
}
