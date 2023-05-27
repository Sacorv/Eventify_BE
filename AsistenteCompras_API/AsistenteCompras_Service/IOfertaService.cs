using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services;

public interface IOfertaService
{
    List<OfertaDTO> ObtenerOfertasMenorPrecioPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida);
    List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);

}
