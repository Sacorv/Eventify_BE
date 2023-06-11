using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services;

public interface IComidaRepository
{
    List<Comidum> ObtenerComida(int idComida);
    List<TipoProductoDTO> ObtenerIngredientes(int idComida);
    int ObtenerCantidadMinimaDeComensales(int idComida);
}
