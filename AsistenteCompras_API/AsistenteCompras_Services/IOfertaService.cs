using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> idLocalidades);
 
        List<OfertaDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad);
    }
}
