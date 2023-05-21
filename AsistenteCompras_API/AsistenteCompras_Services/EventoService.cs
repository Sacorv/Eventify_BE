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

}
