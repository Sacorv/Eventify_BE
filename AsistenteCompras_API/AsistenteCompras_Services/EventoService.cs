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

        public List<Comidum> ObtenerComidas(int idEvento)
        {
            List<Comidum> comidas = new List<Comidum>();

            var comidaEvento = _context.EventoComida
                                        .Where(ev => ev.IdEventoNavigation.Id == idEvento)
                                        .Select(ev => ev.IdComidaNavigation).ToList();

            foreach (Comidum comida in comidaEvento)
            {
                comidas.Add(comida);
            }

            return comidas;
        }

       
    }
}
