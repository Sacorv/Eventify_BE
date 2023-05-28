using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System.Collections;

namespace AsistenteCompras_Repository;

public interface IOfertaRepository
{
    List<OfertaDTO> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida);

    Decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);

    List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, Decimal precio);

    List<Comercio> ComerciosDentroDelRadio(double latitud, double longitud, float distancia);

    List<OfertaDTO> OfertasDentroDelRadio(int idComida, int idBebida, ArrayList idComercios);
}
