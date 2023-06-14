using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _usuarioService;
        private ITokenService _tokenService;

        public UsuarioController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("inicioSesion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult AutenticarUsuario([FromBody]LoginUsuarioDTO usuario)
        {
            try
            {
                Usuario usuarioEncontrado = _usuarioService.IniciarSesion(usuario.Email, usuario.Clave);
                if (usuarioEncontrado != null)
                {
                    //return Ok( new { token = _tokenService.GenerateToken(usuarioEncontrado)});

                    return Ok(usuarioEncontrado);
                }
                else
                {
                    return BadRequest( new { message = "Usuario y/o contraseña son incorrectos"});
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult RegistrarUsuario([FromBody]UsuarioDTO usuario)
        {
            try
            {
                bool validacionClave = _usuarioService.ValidarClaves(usuario.Clave, usuario.ClaveAComparar);

                if (validacionClave == true)
                {
                    Usuario nuevoUsuario = new Usuario();

                    nuevoUsuario.Nombre = usuario.Nombre;
                    nuevoUsuario.Apellido = usuario.Apellido;
                    nuevoUsuario.Email = usuario.Email;
                    nuevoUsuario.Clave = usuario.Clave;
                    
                    string nombreUsuario = _usuarioService.RegistrarUsuario(nuevoUsuario);

                    return Ok(new {message = "Registro: "+ $"{nombreUsuario}"});
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

    }
}
