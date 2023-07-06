using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IComercioRepository
{
    PerfilComercio LoguearComercio(string usuario, string clave);

    Comercio RegistrarComercio(Comercio comercio);

    List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

    string ObtenerImagenComercio(int idComercio);

    List<OfertaComercioDTO> ObtenerOfertasDelComercio(int idComercio);

    List<int> ObtenerComercioIds();

}
