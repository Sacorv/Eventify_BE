using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services;

public interface IOfertaService
{
    List<OfertaDTO> ObtenerOfertasMenorPrecioPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida);
    
    List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);

    List<OfertaDTO> ObtenerOfertasPorZonaGeografica(double latitud, double longitud, float distancia, int idComida, int idBebida);

}
