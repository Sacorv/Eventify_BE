using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IUbicacionRepository
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();

    int BuscarLocalidadPorNombre(string localidad);
}
