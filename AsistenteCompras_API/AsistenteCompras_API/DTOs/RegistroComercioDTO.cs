using System.Globalization;
using System.Runtime.CompilerServices;

namespace AsistenteCompras_API.DTOs
{
    public class RegistroComercioDTO
    {
        public string RazonSocial { get; set; } = null!;

        public string CUIT { get; set; } = null!;
        
        public string Direccion { get; set; } = null!;

        public string Localidad { get; set; } = null!;

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Email { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public string ClaveAComparar { get; set; } = null!;

        public string Imagen { get; set; } = null!;

        public string Rol { get; set; } = null!;
    }
}
