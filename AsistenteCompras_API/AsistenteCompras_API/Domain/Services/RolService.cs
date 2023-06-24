namespace AsistenteCompras_API.Domain.Services
{
    public class RolService : IRolService
    {
        private IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public int BuscarRolPorNombre(string NombreRol)
        {
            return _rolRepository.BuscarRolPorNombre(NombreRol);
        }
    }
}
