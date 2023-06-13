using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;

namespace AsistenteCompras_API.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AsistenteComprasContext _context;
        public UsuarioRepository(AsistenteComprasContext context)
        {
            _context = context;
        }


        public Usuario VerificarUsuario(string email, string clave)
        {
            //Usuario usuario = _context.Usuarios.Where(u => u.Email.Equals(email) && u.Clave.Equals(clave)).First();

            return _context.Usuarios.FirstOrDefault(u => u.Email.Equals(email) && u.Clave.Equals(clave));
        }

        public Usuario RegistrarUsuario(Usuario usuario)
        {
            Usuario usuarioARegistrar = _context.Usuarios.FirstOrDefault(u => u.Email.Equals(usuario.Email));

            if(usuarioARegistrar == null)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return usuario;
            }
            else
            {
                return null;
            }
        }
    }
}
