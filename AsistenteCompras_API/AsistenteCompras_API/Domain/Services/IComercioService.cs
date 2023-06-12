using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IComercioService
{
    List<int> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

    OfertaDTO CompararDistanciaEntreComercios(double latitudUbicacion, double longitudUbicacion, OfertaDTO ofertaComercioUno, OfertaDTO ofertaComercioDos);

    string ObtenerImagenDelComercio(int idComercio);
}
