using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Repository;

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
