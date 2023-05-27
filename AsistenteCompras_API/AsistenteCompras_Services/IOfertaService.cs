using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using Microsoft.Spatial;
using System.Collections;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);
        List<OfertasDTO> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida);
        List<Comercio> ComerciosDentroDelRadio(double latitud, double longitud, float distancia);
        List<OfertasDTO> OfertasDentroDelRadio(int idComida, int idBebida, ArrayList idComercios);
    }
}
