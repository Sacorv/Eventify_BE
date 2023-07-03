using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;


namespace AsistenteCompras_API.Domain.Services;

public class ComercioService : IComercioService
{
    private IComercioRepository _comercioRepository;

    private IOfertaRepository _ofertaRepository;

    public ComercioService(IComercioRepository comercioRepository, IOfertaRepository ofertaRepository)
    {
        _comercioRepository = comercioRepository;
        _ofertaRepository = ofertaRepository;
    }


    public Login IniciarSesion(string email, string clave)
    {
        Login comercio = _comercioRepository.VerificarComercio(email, clave);
        if (comercio != null)
        {
            return comercio;
        }
        else
        {
            return null!;
        }
    }

    public string RegistrarComercio(Comercio comercio)
    {
        Comercio comercioNuevo = _comercioRepository.RegistrarComercio(comercio);
        if (comercioNuevo != null)
        {
            return comercioNuevo.RazonSocial + " - " + comercioNuevo.CUIT;
        }
        else
        {
            return "El email o CUIT ingresado ya se encuentra registrado";
        }
    }

    public bool ValidarClaves(string clave, string claveAComparar)
    {
        return clave.Equals(claveAComparar) ? true : false;
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

    public List<OfertaComercioDTO> ObtenerOfertasDelComercio(int idComercio)
    {
        List<OfertaComercioDTO> ofertasDelComercio;
        try
        {
            ofertasDelComercio = _comercioRepository.ObtenerOfertasDelComercio(idComercio);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        return ofertasDelComercio;
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

    public int CargarOfertaDelComercio(int idComercio, int idProducto, decimal precio, DateTime fechaFin)
    {
        Publicacion oferta = new Publicacion();
        oferta.IdComercio = idComercio;
        oferta.IdProducto = idProducto;
        oferta.Precio = precio;
        oferta.FechaFin = fechaFin;
        oferta.Estado = true;
        return _ofertaRepository.CargarOferta(oferta);
    }

    public bool VerficarSiElComercioExiste(int idComercio)
    {
        return _comercioRepository.VerificarSiElComercioExiste(idComercio);
    }
}
