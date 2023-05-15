using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento);
        
        List<OfertaDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad);

    }
}
