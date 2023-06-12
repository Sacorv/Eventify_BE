using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services;

public interface IEventoRepository
{
    List<Evento> ObtenerEventos();

    List<Comidum> ObtenerComidas(int idEvento);

    List<Bebidum> ObtenerBebidas(int idEvento);

    List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida);

    List<TipoProductoDTO> ObtenerBebidaTipoProductos(int idBebida);

    int ObtenerCantidadMinimaBebidaPorInvitados(int idTipoProducto);
}
