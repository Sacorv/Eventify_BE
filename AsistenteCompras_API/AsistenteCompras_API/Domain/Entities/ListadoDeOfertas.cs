namespace AsistenteCompras_API.Domain.Entities
{
    public class ListadoDeOfertas
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public int CantidadOfertasElegidas { get; set; }

        public double Total { get; set; }

        public bool Estado { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

        public virtual ICollection<OfertaElegida> OfertasElegidas { get; set; } = new List<OfertaElegida>();


    }
}
