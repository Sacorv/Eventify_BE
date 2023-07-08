namespace AsistenteCompras_API.Domain
{
    public class PerfilComercio
    {
        public int Id { get; set; }

        public string Rol { get; set; } = null!;

        public string RazonSocial { get; set; } = null!;

        public string CUIT { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Localidad { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Imagen { get; set; } = null!;
    }
}
