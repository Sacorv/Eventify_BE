namespace AsistenteCompras_Entities.DTOs
{
    public class OfertasPorProductoDTO
    {
        public string nombreProducto { get; set; }
        public List<OfertaCantidadDTO> ofertas { get; set; }
    }
}
