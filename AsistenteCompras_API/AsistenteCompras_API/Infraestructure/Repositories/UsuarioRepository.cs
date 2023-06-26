using AsistenteCompras_API.Domain;
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


        public Login VerificarUsuario(string email, string clave)
        {
            return _context.Usuarios.Where(u => u.Email
                                    .Equals(email) && u.Clave.Equals(clave))
                                    .Select(u => new Login
                                    {
                                        Id = u.Id,
                                        Rol = u.IdRolNavigation.Nombre,
                                        Usuario = u.Nombre + " " + u.Apellido,
                                        Email = u.Email
                                    }).FirstOrDefault()!;

        }

        public Usuario RegistrarUsuario(Usuario usuario)
        {
            Usuario usuarioARegistrar = _context.Usuarios.FirstOrDefault(u => u.Email.Equals(usuario.Email))!;

            if(usuarioARegistrar == null)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return usuario;
            }
            else
            {
                return null!;
            }
        }
    }
}
