using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

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
