using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Repository;

public interface IEventoRepository
{
    List<Evento> ObtenerEventos();

    List<Comidum> ObtenerComidas(int idEvento);

    List<Bebidum> ObtenerBebidasPosibles(int idEvento);

    List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida);
}
