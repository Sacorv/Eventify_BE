namespace AsistenteCompras_API.DTOs
{
    public class ListadoOfertasDTO
    {
        public int IdUsuario { get; set; }

        public int IdEvento { get; set; }

        public List<int> IdBebidas { get; set; } = null!;
        
        public List<int> IdComidas { get; set; } = null!;

        public int CantidadOfertas { get; set; }

        public double Total { get; set; }

        public List<OfertaElegidaDTO> Ofertas { get; set; } = null!;

        public string UrlRecorrido { get; set; } = null!;

        public string MensajeOfertas { get; set; } = null!;

        public double DistanciaARecorrer { get; set; }
    }
}
