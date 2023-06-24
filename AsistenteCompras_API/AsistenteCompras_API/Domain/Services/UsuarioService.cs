using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Repositories;

namespace AsistenteCompras_API.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public UsuarioLogin IniciarSesion(string email, string clave)
        {
            UsuarioLogin usuario = _usuarioRepository.VerificarUsuario(email, clave);
            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }

        public string RegistrarUsuario(Usuario usuario)
        {
            Usuario usuarioNuevo = _usuarioRepository.RegistrarUsuario(usuario);

            if(usuarioNuevo != null)
            {
                return usuarioNuevo.Nombre + " " + usuarioNuevo.Apellido;
            }
            else
            {
                return "El email ingresado ya se encuentra asociado a una cuenta";
            }            
        }

        public bool ValidarClaves(string clave, string claveAComparar)
        {
            return clave.Equals(claveAComparar) ? true : false;
        }
    }
}
