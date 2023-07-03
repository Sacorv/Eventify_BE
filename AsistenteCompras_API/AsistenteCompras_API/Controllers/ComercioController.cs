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

    public ComercioController(IComercioService comercioService, IRolService rolService, IUbicacionService ubicacionService)
    {
        _comercioService = comercioService;
        _rolService = rolService;
        _ubicacionService = ubicacionService;
    }

    [HttpPost("inicioSesion")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoginDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult AutenticarUsuario([FromBody] LoginDTO login)
    {
        try
        {
            Login comercioEncontrado = _comercioService.IniciarSesion(login.Email, login.Clave);
            if (comercioEncontrado != null)
            {
                //return Ok( new { token = _tokenService.GenerateToken(comercioEncontrado)});
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
}
