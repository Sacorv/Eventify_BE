using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.DTOs
{
    public class RegistroUsuarioDTO
    {
        [Required]
        [StringLength(40, ErrorMessage = "La longitud del nombre debe ser menor a 40 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(40, ErrorMessage = "La longitud del apellido debe ser menor a 40 caracteres")]
        public string Apellido { get; set; } = null!;

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
        [StringLength(12, ErrorMessage = "La longitud del rol supera el máximo permitido de caracteres")]
        public string Rol { get; set; } = null!;
    }
}
