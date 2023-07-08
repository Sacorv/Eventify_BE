using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public PerfilUsuario IniciarSesion(string email, string clave)
        {
            PerfilUsuario usuario = _usuarioRepository.VerificarUsuario(email, clave);
            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return null!;
            }
        }

        public Usuario RegistrarUsuario(Usuario usuario)
        {
            return _usuarioRepository.RegistrarUsuario(usuario);            
        }

        public bool ValidarClaves(string clave, string claveAComparar)
        {
            return clave.Equals(claveAComparar) ? true : false;
        }
    }
}
