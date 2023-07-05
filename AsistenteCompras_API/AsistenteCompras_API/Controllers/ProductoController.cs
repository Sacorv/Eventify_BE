using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private IProductoService _productoService;
    private ITipoProductoService _tipoProductoService;

    public ProductoController(IProductoService productoService, ITipoProductoService tipoProductoService)
    {
        _productoService = productoService;
        _tipoProductoService = tipoProductoService;
    }

    [HttpGet("tipoproductos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult TiposProductoDisponible()
    {
        List<TipoProductoDTO> productosDisponibles;
        try
        {
            productosDisponibles = _tipoProductoService.ObtenerTodosLosTiposDeProductos();
            if (productosDisponibles.IsNullOrEmpty())
                return NotFound("No hay productos en la plataforma");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        return Ok(productosDisponibles);
    }

    [HttpGet("marcas/{idTipoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<String>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult MarcasDisponibles(int idTipoProducto)
    {
        List<String> marcas;
        try
        {
            marcas = _productoService.ObtenerMarcasParaElTipoDeProducto(idTipoProducto);
            if (marcas.IsNullOrEmpty())
                return NotFound("No se encontraron marcas para el tipo de producto");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        return Ok(marcas);
    }

    [HttpGet("productos/{idTipoProducto}/{marca}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult ProductosPorMarcaYTipo(int idTipoProducto, string marca)
    {
        List<ProductoDTO> productos;
        try
        {
            productos = _productoService.ObtenerProductosPorMarcaYTipoProducto(idTipoProducto, marca);
            if (productos.IsNullOrEmpty())
                return NotFound("No se encontraron productos");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        return Ok(productos);
    }

    [HttpGet("producto/{codigoBarras}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult ProductoPorCodigoBarra(string codigoBarras)
    {
        ProductoDTO producto;
        try
        {
            producto = _productoService.ObtenerProductoPorCodigoDeBarras(codigoBarras);
            if (producto == null)
                return NotFound("No se encontro el producto");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        return Ok(producto);
    }
}
