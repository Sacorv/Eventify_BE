using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IListadoOfertasRepository
    {
        int GuardarListado(ListadoDeOfertas listado);

        void GuardarOfertaEnListado(OfertaElegida oferta);

        List<ListadoDeOfertas> ObtenerListados();

        ListadoDeOfertas BuscarListado(int idListado);

        void ModificarListado(ListadoDeOfertas listado);
    }
}
