namespace AsistenteCompras_Entities.DTOs
{
    public class OfertaDTO
    {
        public string NombreProducto { get; set; } = null!;

        public string Marca { get; set; } = null!;

        public string Imagen { get; set; } = null!;

        public decimal Precio { get; set; }

        public string NombreComercio { get; set; } = null!;

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }

        public string Localidad { get; set; } = null!;
    }
}
