using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);

        List<OfertaDTOPrueba> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida);


    }
}
