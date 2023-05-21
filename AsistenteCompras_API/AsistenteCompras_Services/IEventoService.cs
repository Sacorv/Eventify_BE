using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services
{
    public interface IEventoService
    {
        List<Evento> ObtenerEventos();

        List<Comidum> ObtenerComidas(int idEvento);


    }
}
