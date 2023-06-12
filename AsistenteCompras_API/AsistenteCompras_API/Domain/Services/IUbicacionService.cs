using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IUbicacionService
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();
}
