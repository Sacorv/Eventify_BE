using AsistenteCompras_Entities.DTOs;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento);
    }
}
