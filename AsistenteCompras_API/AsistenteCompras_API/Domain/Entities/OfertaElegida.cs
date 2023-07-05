namespace AsistenteCompras_API.Domain.Entities
{
    public class OfertaElegida
    {
        public int Id { get; set; }

        public string? NombreProducto { get; set; }

        public int IdPublicacion { get; set; }

        public double Precio { get; set; }

        public int Cantidad { get; set; }

        public double Subtotal { get; set; }

        public bool Estado { get; set; }

        public int IdListadoDeOfertas { get; set; }

        public virtual Publicacion IdPublicacionNavigation { get; set; } = null!;
        public virtual ListadoDeOfertas IdListadoDeOfertasNavigation { get; set; } = null!;

    }
}
