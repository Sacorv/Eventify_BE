namespace AsistenteCompras_Entities.Entities;

public partial class EventoComidum
{
    public int Id { get; set; }
    public int IdComida { get; set; }

    public int IdEvento { get; set; }

    public virtual Comidum IdComidaNavigation { get; set; } = null!;

    public virtual Evento IdEventoNavigation { get; set; } = null!;
}
