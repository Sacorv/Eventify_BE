using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.Domain
{
    public class Listado
    {
        [Required(ErrorMessage = "Id de usuario es requerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Id de evento es requerido")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "Id de bebida es requerido")]
        public List<int> IdBebidas { get; set; } = null!;

        [Required(ErrorMessage = "Id de comida es requerido")]
        public List<int> IdComidas { get; set; } = null!;

        [Required(ErrorMessage = "Cantidad de ofertas es requerido")]
        public int CantidadOfertas { get; set; }

        [Required(ErrorMessage = "Total es requerido")]
        public double Total { get; set; }

        [Required(ErrorMessage = "Listado de ofertas es requerido")]
        public List<OfertaSeleccionada> Ofertas { get; set; } = null!;

        [Required(ErrorMessage = "URL de recorrido es requerido")]
        public string UrlRecorrido { get; set; } = null!;

        [Required(ErrorMessage = "Mensaje es requerido")]
        public string MensajeOfertas { get; set; } = null!;

        [Required(ErrorMessage = "Distancia a recorrer es requerido")]
        public double DistanciaARecorrer { get; set; }
    }
}
