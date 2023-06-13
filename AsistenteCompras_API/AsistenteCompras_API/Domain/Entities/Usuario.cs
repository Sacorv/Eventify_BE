namespace AsistenteCompras_API.Domain.Entities
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string ?Email { get; set; } = null;

        public string Clave { get; set; } = null!;
    }
}
