using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public class UbicacionService : IUbicacionService
{
    private IUbicacionRepository _ubicacionRepository;

    public UbicacionService(IUbicacionRepository ubicacionRepository)
    {
        _ubicacionRepository = ubicacionRepository;
    }

    public double CalcularDistanciaPorHaversine(double latitudOrigen, double longitudOrigen, double latitudDestino, double longitudDestino)
    {
        const double RADIO_TIERRA_KILOMETROS = 6371;

        double latitudRadianes1 = ConvertirARadianes(latitudOrigen);
        double longitudRadianes1 = ConvertirARadianes(longitudOrigen);
        double latitudRadianes2 = ConvertirARadianes(latitudDestino);
        double longitudRadianes2 = ConvertirARadianes(longitudDestino);

        double diferenciaLatitud = latitudRadianes2 - latitudRadianes1;
        double diferenciaLongitud = longitudRadianes2 - longitudRadianes1;

        double a = Math.Pow(Math.Sin(diferenciaLatitud / 2), 2) + Math.Cos(latitudRadianes1) * Math.Cos(latitudRadianes2) * Math.Pow(Math.Sin(diferenciaLongitud / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distancia = RADIO_TIERRA_KILOMETROS * c;

        return distancia;
    }

    public List<LocalidadDTO> ObtenerTodasLasLocalidades()
    {
        return _ubicacionRepository.ObtenerTodasLasLocalidades();
    }

    public int BuscarLocalidadPorNombre(string localidad)
    {
        

        return _ubicacionRepository.BuscarLocalidadPorNombre(localidad);
    }

    private static double ConvertirARadianes(double grados)
    {
        return grados * Math.PI / 180;
    }
}
