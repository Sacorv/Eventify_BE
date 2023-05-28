using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Infraestructure.Repositories;

public interface IUbicacionRepository
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();
}
