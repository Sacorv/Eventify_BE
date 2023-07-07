using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AsistenteCompras_API.DTOs
{
    public class RegistroComercioDTO
    {
        [Required]
        [StringLength(40, ErrorMessage = "La longitud de la razón social debe ser menor a 40 caracteres")]
        public string RazonSocial { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(20|23|27|30|33)[0-9]{8}[0-9]", ErrorMessage = "El formato del CUIT es incorrecto")]
        [StringLength(11, ErrorMessage = "La longitud del CUIT debe ser igual a 11 caracteres")]
        public string CUIT { get; set; } = null!;

        [Required]
        [StringLength(40, ErrorMessage = "La longitud de dirección debe ser menor a 40 caracteres")]
        public string Direccion { get; set; } = null!;

        [Required]
        [StringLength(40, ErrorMessage = "La longitud de localidad debe ser menor a 40 caracteres")]
        public string Localidad { get; set; } = null!;

        [Required]
        [NotNull]
        public double Latitud { get; set; }

        [Required]
        [NotNull]
        public double Longitud { get; set; }

        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "El formato del email es incorrecto")]
        [StringLength(50, ErrorMessage = "La longitud del email debe ser menor a 50 caracteres")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(16, ErrorMessage = "La longitud de la contraseña debe ser menor a 16 caracteres")]
        public string Clave { get; set; } = null!;

        [Required]
        [MaxLength(16, ErrorMessage = "La longitud de la contraseña debe ser menor a 16 caracteres")]
        public string ClaveAComparar { get; set; } = null!;

        [Required]
        public string Imagen { get; set; } = null!;

        [Required]
        [StringLength(12, ErrorMessage = "La longitud del rol supera el máximo permitido de caracteres")]
        public string Rol { get; set; } = null!;
    }
}
