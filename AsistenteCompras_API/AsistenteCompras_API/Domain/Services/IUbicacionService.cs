using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IUbicacionService
{
    List<LocalidadDTO> ObtenerTodasLasLocalidades();

    int BuscarLocalidadPorNombre(string localidad);

    double CalcularDistanciaPorHaversine(double latitudOrigen, double longitudOrigen, double latitudDestino, double longitudDestino);
}
