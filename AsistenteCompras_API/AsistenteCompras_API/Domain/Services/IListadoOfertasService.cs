using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IListadoOfertasService
    {
        int GuardarListadoConOfertas(Listado listado);

        List<ListadosUsuario> ConsultarListados(int idUsuario);

        ListadoOfertasUsuario BuscarListado(int idListado, int idUsuario);
    }
}
