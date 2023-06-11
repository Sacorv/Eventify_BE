using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Infraestructure.Repositories;

public interface IEventoRepository
{
    List<Evento> ObtenerEventos();

    List<Comidum> ObtenerComidas(int idEvento);

    List<Bebidum> ObtenerBebidas(int idEvento);

    List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida);

    List<TipoProductoDTO> ObtenerBebidaTipoProductos(int idBebida);

    int ObtenerCantidadMinimaBebidaPorInvitados(int idTipoProducto);
}
