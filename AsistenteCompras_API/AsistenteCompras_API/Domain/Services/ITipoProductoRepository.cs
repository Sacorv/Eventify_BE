using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface ITipoProductoRepository
{
    List<int> ObtenerIdTipoProductosComida(int idComida);
    List<int> ObtenerIdTipoProductosBebida(int idBebida);
    List<int> ObtenerIdTipoProductosBebidaV2(List<int> idBebida);
    List<int> ObtenerIdTipoProductosComidaV2(List<int> idComida);
    List<TipoProductoDTO> ObtenerTodosLosTiposDeProductos();
}
