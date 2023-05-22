using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento);

        List<OfertaDTOPrueba> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida);

    }
}
