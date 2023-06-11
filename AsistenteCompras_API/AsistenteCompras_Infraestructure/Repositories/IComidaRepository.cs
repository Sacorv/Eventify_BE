using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Infraestructure.Repositories;

public interface IComidaRepository
{
    List<Comidum> ObtenerComida(int idComida);
    List<TipoProductoDTO> ObtenerIngredientes(int idComida);
    int ObtenerCantidadMinimaDeComensales(int idComida);
}
