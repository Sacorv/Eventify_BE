using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.DTOs
{
    public class ListadoOfertasDTO
    {
        public int IdUsuario { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public List<OfertaElegidaDTO> Ofertas { get; set; }
    }
}
