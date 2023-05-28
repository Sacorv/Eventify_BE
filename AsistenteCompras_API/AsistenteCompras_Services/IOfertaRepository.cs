using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System.Collections;

namespace AsistenteCompras_Repository;

public interface IOfertaRepository
{
    List<OfertaDTO> OfertasPorLocalidad(int idLocalidad, List<int> idProductos);

    Decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);

    List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, Decimal precio);

    List<Comercio> ComerciosDentroDelRadio(double latitud, double longitud, float distancia);

    List<OfertaDTO> OfertasDentroDelRadio(List<int> idProductos, List<int> idComercios);
}
