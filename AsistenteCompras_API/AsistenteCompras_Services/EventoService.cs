using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services;

public class EventoService : IEventoService
{

    private AsistenteComprasContext _context;

    public EventoService(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<Bebidum> ObtenerBebidasPosibles(int idEvento)
    {
        return _context.EventoBebida.Where(e => e.IdEvento.Equals(idEvento))
                                    .Select(b => new Bebidum
                                    {
                                        Id = b.IdBebida,
                                        TipoBebida = b.IdBebidaNavigation.TipoBebida
                                    }).ToList();
    }

    public List<Evento> ObtenerEventos()
    {
        return _context.Eventos.ToList();
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
