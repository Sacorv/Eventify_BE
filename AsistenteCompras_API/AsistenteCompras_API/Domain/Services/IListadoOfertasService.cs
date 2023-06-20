using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IListadoOfertasService
    {
        int GuardarListadoConOfertas(ListadoOfertasDTO listado);

        List<ListadoDeOfertas> ConsultarListados();

        ListadoOfertasUsuario BuscarListado(int idListado, int idUsuario);

        void ModificarListado(ListadoDeOfertas listado);

    }
}
