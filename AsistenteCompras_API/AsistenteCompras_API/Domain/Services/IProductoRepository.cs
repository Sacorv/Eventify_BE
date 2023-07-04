using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IProductoRepository
{
    ProductoDTO ObtenerProductoPorCodigoDeBarras(string codigoBarras);

    List<String> ObtenerMarcasParaElTipoDeProducto(int tipoProducto);

    List<ProductoDTO> ObtenerProductosPorMarcaYTipoProducto(int tipoProducto, string marca);

    List<int> ObtenerProductoIds();
}
