namespace AsistenteCompras_API.Domain
{
    public class OfertaSeleccionada
    {
        public string? NombreProducto { get; set; }

        public int IdPublicacion { get; set; }

        public double Precio { get; set; }

        public int Cantidad { get; set; }

        public double Subtotal { get; set; }
    }
}
