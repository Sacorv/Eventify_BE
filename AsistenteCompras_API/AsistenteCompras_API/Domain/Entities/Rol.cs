namespace AsistenteCompras_API.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
