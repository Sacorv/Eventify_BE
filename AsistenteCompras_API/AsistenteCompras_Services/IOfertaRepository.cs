using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Repository;

public interface IOfertaRepository
{
    List<OfertaDTO> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida);

    Decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);

    List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, Decimal precio);
}
