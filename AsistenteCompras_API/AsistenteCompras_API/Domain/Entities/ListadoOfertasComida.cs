namespace AsistenteCompras_API.Domain.Entities
{
    public partial class ListadoOfertasComida
    {
        public int Id { get; set; }

        public int IdListadoDeOfertas { get; set; }

        public int IdComida { get; set; }

        public virtual ListadoDeOfertas IdListadoDeOfertasNavigation { get; set; } = null!;

        public virtual Comidum IdComidaNavigation { get; set; } = null!;
    }
}
