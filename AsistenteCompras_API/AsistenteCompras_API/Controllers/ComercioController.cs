using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComercioController : ControllerBase
{
    private IComercioService _comercioService;
    private IRolService _rolService;
    private IUbicacionService _ubicacionService;
    private IProductoService _productoService;
    private IOfertaService _ofertaService;

    public ComercioController(IComercioService comercioService, IRolService rolService, IUbicacionService ubicacionService, IProductoService productoService, IOfertaService ofertaService)
    {
        _comercioService = comercioService;
        _rolService = rolService;
        _ubicacionService = ubicacionService;
        _productoService = productoService;
        _ofertaService = ofertaService;
    }

    [HttpPost("inicioSesion")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilComercio))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult AutenticarComercio([FromBody] LoginDTO login)
    {
        try
        {
            PerfilComercio comercioEncontrado = _comercioService.IniciarSesion(login.Email, login.Clave);
            if (comercioEncontrado != null)
            {
                return Ok(comercioEncontrado);
            }
            else
            {
                return BadRequest(new { message = "Email y/o contraseña son incorrectos" });
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }


    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistroComercioDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult RegistrarComercio([FromBody] RegistroComercioDTO comercio)
    {
        try
        {
            bool validacionClave = _comercioService.ValidarClaves(comercio.Clave, comercio.ClaveAComparar);

            if (validacionClave == true)
            {
                int idRol = _rolService.BuscarRolPorNombre(comercio.Rol);
                int idLocalidad = _ubicacionService.BuscarLocalidadPorNombre(comercio.Localidad);
                string resultado = "";

                if(idRol!= 0 && idLocalidad != 0)
                {
                    Comercio nuevoComercio = new Comercio();

                    nuevoComercio.RazonSocial = comercio.RazonSocial;
                    nuevoComercio.CUIT = comercio.CUIT;
                    nuevoComercio.Direccion = comercio.Direccion;
                    nuevoComercio.IdLocalidad = idLocalidad;
                    nuevoComercio.Latitud = (decimal)comercio.Latitud;
                    nuevoComercio.Longitud = (decimal)comercio.Longitud;
                    nuevoComercio.Email = comercio.Email;
                    nuevoComercio.Clave = comercio.Clave;
                    nuevoComercio.Imagen = comercio.Imagen;
                    nuevoComercio.IdRol = idRol;

                    resultado = _comercioService.RegistrarComercio(nuevoComercio);
                }
                return Ok(new { message = "Registro: " + $"{resultado}" });
            }
            else
            {
                return BadRequest(new { message = "Las claves no coinciden" });
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpGet("VerOfertas/{idComercio}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaComercioDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult VerOfertasDelComercio(int idComercio)
    {
        List<OfertaComercioDTO> ofertasDelComercio;
        try
        {
            ofertasDelComercio = _comercioService.ObtenerOfertasDelComercio(idComercio);
            if (ofertasDelComercio.IsNullOrEmpty())
            {
                return NotFound("No hay ofertas para el comercio");
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return Ok(ofertasDelComercio);
    }

    [HttpPost("cargarOferta")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult CargarOfertaDelComercio([FromBody] OfertaAPublicarDTO oferta)
    {
        try
        {
            if(!(_comercioService.VerficarSiElComercioExiste(oferta.IdComercio)))
                return NotFound("El comercio no se encuentra en la plataforma");
            if (!(_productoService.VerificarSiElProductoExiste(oferta.IdProducto)))
                return NotFound("El producto no se encuentra en la plataforma");
            if (_ofertaService.VerficarSiLaOfertaExiste(oferta.IdComercio, oferta.IdProducto))
                return BadRequest("La oferta ya se encuentra en la plataforma");
            return Ok(_comercioService.CargarOfertaDelComercio(oferta.IdComercio,oferta.IdProducto,oferta.Precio,oferta.FechaFin));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
