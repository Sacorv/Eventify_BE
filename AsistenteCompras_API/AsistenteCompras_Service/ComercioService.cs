using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Services
{
    public class ComercioService : IComercioService
    {
        private IComercioRepository _comercioRepository;

        public ComercioService(IComercioRepository comercioRepository)
        {
            _comercioRepository = comercioRepository;
        }

        public List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia)
        {
            return _comercioRepository.ObtenerComerciosPorRadio(latitud, longitud, distancia);
        }

        public OfertaDTO CompararDistanciaEntreComercios(double latitudUbicacion, double longitudUbicacion, OfertaDTO ofertaComercioUno, OfertaDTO ofertaComercioDos)
        {
            double distanciaKmComercioUno = CalcularDistanciaHaversine(latitudUbicacion, longitudUbicacion, ofertaComercioUno.Latitud, ofertaComercioUno.Longitud);

            double distanciaKmComercioDos = CalcularDistanciaHaversine(latitudUbicacion, longitudUbicacion, ofertaComercioDos.Latitud, ofertaComercioDos.Longitud);

            if (distanciaKmComercioUno < distanciaKmComercioDos)
            {
                return ofertaComercioUno;
            }
            else
            {
                return ofertaComercioDos;
            }

        }

        private static double CalcularDistanciaHaversine(double latitudUno, double longitudUno, double latitudDos, double longitudDos)
        {
            const double radioTierraKilometros = 6371;

            double latitudRadianes1 = ConvertirARadianes(latitudUno);
            double longitudRadianes1 = ConvertirARadianes(longitudUno);
            double latitudRadianes2 = ConvertirARadianes(latitudDos);
            double longitudRadianes2 = ConvertirARadianes(longitudDos);

            double diferenciaLatitud = latitudRadianes2 - latitudRadianes1;
            double diferenciaLongitud = longitudRadianes2 - longitudRadianes1;

            // Fórmula del haversine
            double a = Math.Pow(Math.Sin(diferenciaLatitud / 2), 2) + Math.Cos(latitudRadianes1) * Math.Cos(latitudRadianes2) * Math.Pow(Math.Sin(diferenciaLongitud / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distancia = radioTierraKilometros * c;

            return distancia;
        }

        private static double ConvertirARadianes(double grados)
        {
            return grados * Math.PI / 180;
        }

    }
}
