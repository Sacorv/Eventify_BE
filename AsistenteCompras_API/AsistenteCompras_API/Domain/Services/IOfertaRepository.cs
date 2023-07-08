using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services;

public interface IOfertaRepository
{
    List<Oferta> OfertasPorLocalidad(int idLocalidad, List<int> idProductos);
    decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);
    List<Oferta> ObtenerOfertasPorPrecio(int idTipoProducto, decimal precio);
    List<Oferta> OfertasDentroDelRadio(List<int> idProductos, List<int> idComercios);
    List<Oferta> OfertasDentroDelRadioV2(List<int> idProductos, List<int> idComercios, List<String> marcasElegidas);
    List<String> ObtenerMarcasComidasDisponibles(List<int> idProductos);
    List<String> ObtenerMarcasBebidasDisponibles(List<int> idProductos);
    List<Oferta> OfertasPorComercioFiltradasPorFecha(int idComercio, DateTime fecha);
    int CargarOferta(Publicacion oferta);
    List<int> ObtenerIdsProductosDelComercio(int idComercio);

}
