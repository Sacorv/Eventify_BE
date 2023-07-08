using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.DTOs
{
    public class LoginDTO
    {
        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "El formato del email es incorrecto")]
        [StringLength(50, ErrorMessage = "La longitud del email debe ser menor a 50 caracteres")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(16, ErrorMessage = "La longitud de la contraseña debe ser menor a 16 caracteres")]
        public string Clave { get; set; } = null!;
    }
}
