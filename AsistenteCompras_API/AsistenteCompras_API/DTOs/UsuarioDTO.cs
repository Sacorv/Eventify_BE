namespace AsistenteCompras_API.DTOs
{
    public class UsuarioDTO
    {
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public string ClaveAComparar { get; set; } = null!;
    }
}
