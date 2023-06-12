using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services;

public interface IComercioRepository
{
    List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

}
