using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System.Collections;

namespace AsistenteCompras_Infraestructure.Repositories;

public interface IOfertaRepository
{
    List<OfertaDTO> OfertasPorLocalidad(int idLocalidad, List<int> idProductos);
    decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);
    List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, decimal precio);
    List<OfertaDTO> OfertasDentroDelRadio(List<int> idProductos, List<int> idComercios);
    List<OfertaDTO> OfertasDentroDelRadioV2(List<int> idProductos, List<int> idComercios, List<String> marcasElegidas);
}
