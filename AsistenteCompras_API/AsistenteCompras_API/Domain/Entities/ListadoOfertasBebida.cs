namespace AsistenteCompras_API.Domain.Entities
{
    public partial class ListadoOfertasBebida
    {
        public int Id { get; set; }
        public int IdListadoDeOfertas { get; set; }

        public int IdBebida { get; set; }

        public virtual ListadoDeOfertas IdListadoDeOfertasNavigation { get; set; } = null!;

        public virtual Bebidum IdBebidaNavigation { get; set; } = null!;
    }
}
