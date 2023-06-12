using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;


namespace AsistenteCompras_API.Domain.Services;

public class ComercioService : IComercioService
{
    private IComercioRepository _comercioRepository;

    public ComercioService(IComercioRepository comercioRepository)
    {
        _comercioRepository = comercioRepository;
    }

    public List<int> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia)
    {
        List<int> idComercios = new List<int>();

        List<Comercio> comercios = _comercioRepository.ObtenerComerciosPorRadio(latitud, longitud, distancia);

        comercios.ForEach(c => idComercios.Add(c.Id));

        return idComercios;
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

    public string ObtenerImagenDelComercio(int idComercio)
    {
        return _comercioRepository.ObtenerImagenComercio(idComercio);
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
