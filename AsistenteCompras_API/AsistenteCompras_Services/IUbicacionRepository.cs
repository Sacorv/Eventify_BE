using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Repository;

public interface IUbicacionRepository
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();
}
