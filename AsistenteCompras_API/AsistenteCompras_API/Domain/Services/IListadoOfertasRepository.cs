using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

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

        List<OfertaCantidadDTO> BuscarOfertasAsociadas(int idListado);

        void ModificarListado(ListadoDeOfertas listado);
    }
}
