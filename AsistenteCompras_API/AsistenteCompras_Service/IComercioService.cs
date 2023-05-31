using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services
{
    public interface IComercioService
    {
        List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

        OfertaDTO CompararDistanciaEntreComercios(double latitudUbicacion, double longitudUbicacion, OfertaDTO ofertaComercioUno, OfertaDTO ofertaComercioDos);
    }
}
