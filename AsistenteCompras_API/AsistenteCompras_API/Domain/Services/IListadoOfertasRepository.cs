using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IListadoOfertasRepository
    {
        int GuardarListado(ListadoDeOfertas listado);

        void GuardarOfertaEnListado(OfertaElegida oferta);

        List<ListadosUsuario> ObtenerListados(int idUsuario);

        ListadoOfertasUsuario BuscarListado(int idListado, int idUsuario);

        void GuardarComida(ListadoOfertasComida listadoComidaTipo);

        void GuardarBebida(ListadoOfertasBebida listadoBebidaTipo);

        List<OfertaCantidad> BuscarOfertasAsociadas(int idListado);

        void ModificarListado(ListadoDeOfertas listado);
    }
}
