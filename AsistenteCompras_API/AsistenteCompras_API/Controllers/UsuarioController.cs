using AsistenteCompras_API.Domain;
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
        private IRolService _rolService;

        public UsuarioController(IUsuarioService usuarioService, ITokenService tokenService, IRolService rolService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _rolService = rolService;
        }

        [HttpPost("inicioSesion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilUsuario))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult AutenticarUsuario([FromBody]LoginDTO usuario)
        {
            try
            {
                PerfilUsuario usuarioEncontrado = _usuarioService.IniciarSesion(usuario.Email, usuario.Clave);
                if (usuarioEncontrado != null)
                {
                    //return Ok( new { token = _tokenService.GenerateToken(usuarioEncontrado)});
                    return Ok(usuarioEncontrado);
                }
                else
                {
                    return BadRequest( new { message = "Email y/o contraseña son incorrectos"});
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistroUsuarioDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult RegistrarUsuario([FromBody]RegistroUsuarioDTO usuario)
        {
            try
            {
                bool validacionClave = _usuarioService.ValidarClaves(usuario.Clave, usuario.ClaveAComparar);

                if (validacionClave == true)
                {
                    int idRol = _rolService.BuscarRolPorNombre(usuario.Rol);
                    Usuario nuevoUsuario = new Usuario();

                    nuevoUsuario.Nombre = usuario.Nombre;
                    nuevoUsuario.Apellido = usuario.Apellido;
                    nuevoUsuario.Email = usuario.Email;
                    nuevoUsuario.Clave = usuario.Clave;
                    nuevoUsuario.IdRol = idRol;
                    
                    string resultado = _usuarioService.RegistrarUsuario(nuevoUsuario);

                    return Ok(new {message = "Registro: "+ $"{resultado}"});
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
