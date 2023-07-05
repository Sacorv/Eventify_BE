using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;

namespace AsistenteCompras_API.Infraestructure.Repositories
{
    public class RolRepository : IRolRepository

    {
        private AsistenteComprasContext _context;
        public RolRepository(AsistenteComprasContext context)
        {
            _context = context;
        }


        public int BuscarRolPorNombre(string nombreRol)
        {
            Rol rol = _context.Rol.FirstOrDefault(r => r.Nombre.Equals(nombreRol));

            return rol.Id;
        }
    }
}
