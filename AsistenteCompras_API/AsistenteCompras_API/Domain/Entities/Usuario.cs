namespace AsistenteCompras_API.Domain.Entities
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string ?Email { get; set; } = null;

        public string Clave { get; set; } = null!;

        public int IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; } = null!;

        public virtual ICollection<ListadoDeOfertas> ListadoDeOfertas { get; set; } = new List<ListadoDeOfertas>();
    }
}
