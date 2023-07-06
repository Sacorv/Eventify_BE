using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IComercioService
{
    PerfilComercio IniciarSesion(string email, string clave);

    string RegistrarComercio(Comercio comercio);

    bool ValidarClaves(string clave, string claveAComparar);

    List<int> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

    OfertaDTO CompararDistanciaEntreComercios(double latitudUbicacion, double longitudUbicacion, OfertaDTO ofertaComercioUno, OfertaDTO ofertaComercioDos);

    string ObtenerImagenDelComercio(int idComercio);

    List<OfertaComercioDTO> ObtenerOfertasDelComercio(int idComercio);

    int CargarOfertaDelComercio(int idComercio, int idProducto, decimal precio, DateTime fechaFin);

    bool VerficarSiElComercioExiste(int idComercio);
}
