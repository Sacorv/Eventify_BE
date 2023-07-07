namespace AsistenteCompras_API.DTOs
{
    public class OfertaElegidaDTO
    {
        public string NombreProducto { get; set; } = null!;

        public int IdPublicacion { get; set; }

        public double Precio { get; set; }

        public int Cantidad { get; set; }

        public double Subtotal { get; set; }
    }
}
