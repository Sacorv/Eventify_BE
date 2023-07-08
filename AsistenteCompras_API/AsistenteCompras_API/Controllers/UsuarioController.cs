using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private IUsuarioService _usuarioService;
    private IRolService _rolService;

    public UsuarioController(IUsuarioService usuarioService, IRolService rolService)
    {
        _usuarioService = usuarioService;
        _rolService = rolService;
    }

    [HttpPost("inicioSesion")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilUsuario))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public dynamic AutenticarUsuario([FromBody] LoginDTO login)
    {
        try
        {
            PerfilUsuario usuario = _usuarioService.IniciarSesion(login.Email, login.Clave);
            if (usuario != null)
            {
                return new
                {
                    statusCode = StatusCodes.Status200OK,
                    usuario
                };
            }
            else
            {
                return new 
                { 
                    statusCode = StatusCodes.Status204NoContent,
                    message = "Email y/o contraseña son incorrectos" 
                };
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }


    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistroUsuarioDTO))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public dynamic RegistrarUsuario([FromBody]RegistroUsuarioDTO usuario)
    {
        try
        {
            bool validacionClave = _usuarioService.ValidarClaves(usuario.Clave, usuario.ClaveAComparar);
            Usuario usuarioRegistrado = null;
            if (validacionClave == true)
            {
                int idRol = _rolService.BuscarRolPorNombre(usuario.Rol);
                if (idRol != 0)
                {
                    usuarioRegistrado = nuevoUsuario(usuario, idRol);
                }
                if (usuarioRegistrado != null)
                {
                    return new
                    {
                        statusCode = StatusCodes.Status200OK,
                        usuario = usuarioRegistrado.Nombre + " " + usuarioRegistrado.Apellido,
                        email = usuarioRegistrado.Email
                    };
                }
                else
                {
                    return new
                    {
                        statusCode = StatusCodes.Status204NoContent,
                        message = "El email ingresado ya se encuentra asociado a una cuenta"
                    };
                }
            }
            else
            {
                return new
                {
                    statusCode = StatusCodes.Status204NoContent,
                    message = "Las claves no coinciden"
                };
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    private Usuario nuevoUsuario(RegistroUsuarioDTO usuario, int idRol)
    {
        Usuario nuevoUsuario = new Usuario();

        nuevoUsuario.Nombre = usuario.Nombre;
        nuevoUsuario.Apellido = usuario.Apellido;
        nuevoUsuario.Email = usuario.Email;
        nuevoUsuario.Clave = usuario.Clave;
        nuevoUsuario.IdRol = idRol;

        return _usuarioService.RegistrarUsuario(nuevoUsuario);
    }

}
