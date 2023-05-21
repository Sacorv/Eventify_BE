using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services;

public interface IUbicacionService
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();
}
