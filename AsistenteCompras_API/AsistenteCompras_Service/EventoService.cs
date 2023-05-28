using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _eventoRepository;

    public EventoService(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    public List<Bebidum> ObtenerBebidas(int idEvento)
    {
        return _eventoRepository.ObtenerBebidas(idEvento);
    }

    public List<Comidum> ObtenerComidas(int idEvento)
    {
        return _eventoRepository.ObtenerComidas(idEvento);
    }

    public List<Evento> ObtenerEventos()
    {
        return _eventoRepository.ObtenerEventos();
    }

    public List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida)
    {
        return _eventoRepository.ObtenerListadoParaEvento(idEvento, idComida, idBebida);
    }
}
