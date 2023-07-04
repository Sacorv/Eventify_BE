using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public List<string> ObtenerMarcasParaElTipoDeProducto(int tipoProducto)
    {
        return _productoRepository.ObtenerMarcasParaElTipoDeProducto(tipoProducto);
    }

    public ProductoDTO ObtenerProductoPorCodigoDeBarras(string codigoBarras)
    {
        return _productoRepository.ObtenerProductoPorCodigoDeBarras(codigoBarras);
    }

    public List<ProductoDTO> ObtenerProductosPorMarcaYTipoProducto(int tipoProducto, string marca)
    {
        
        return _productoRepository.ObtenerProductosPorMarcaYTipoProducto(tipoProducto, marca);
    }

    public bool VerificarSiElProductoExiste(int idProducto)
    {
        List<int> idProductos = _productoRepository.ObtenerProductoIds();

        return idProductos.Contains(idProducto);
    }
}
