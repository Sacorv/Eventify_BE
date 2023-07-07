namespace AsistenteCompras_API.Domain
{
    public class PerfilUsuario
    {
        public int Id { get; set; }

        public string Rol { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
