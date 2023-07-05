using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IProductoService
{
    ProductoDTO ObtenerProductoPorCodigoDeBarras(string codigoBarras);

    List<String> ObtenerMarcasParaElTipoDeProducto(int tipoProducto);

    List<ProductoDTO> ObtenerProductosPorMarcaYTipoProducto(int tipoProducto, string marca);

    bool VerificarSiElProductoExiste(int idProducto);
}
