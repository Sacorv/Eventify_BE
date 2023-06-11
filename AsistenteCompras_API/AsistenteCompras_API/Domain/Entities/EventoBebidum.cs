namespace AsistenteCompras_API.Domain.Entities;

public partial class EventoBebidum
{
    public int Id { get; set; }
    public int IdBebida { get; set; }

    public int IdEvento { get; set; }

    public virtual Bebidum IdBebidaNavigation { get; set; } = null!;

    public virtual Evento IdEventoNavigation { get; set; } = null!;
}
