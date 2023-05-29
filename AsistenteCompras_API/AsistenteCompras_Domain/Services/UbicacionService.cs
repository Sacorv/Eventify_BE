using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Services;

public class UbicacionService : IUbicacionService
{
    private IUbicacionRepository _ubicacionRepository;

    public UbicacionService(IUbicacionRepository ubicacionRepository)
    {
        _ubicacionRepository = ubicacionRepository;
    }

    public List<LocalidadDTO> ObtenerTodasLasLocalidades()
    {
        return _ubicacionRepository.ObtenerTodasLasLocalidades();
    }
}
