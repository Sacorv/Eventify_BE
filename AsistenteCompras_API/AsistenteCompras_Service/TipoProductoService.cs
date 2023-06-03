using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private ITipoProductoRepository _tipoProductoRepository;

        public TipoProductoService(ITipoProductoRepository tipoProductoRepository)
        {
            _tipoProductoRepository = tipoProductoRepository;
        }

        public List<int> ObtenerIdTipoProductosBebida(int idBebida)
        {
            return _tipoProductoRepository.ObtenerIdTipoProductosBebida(idBebida);
        }

        public List<int> ObtenerIdTipoProductosComida(int idComida)
        {
            return _tipoProductoRepository.ObtenerIdTipoProductosComida(idComida);
        }

        public List<int> ObtenerIdTipoProductosBebidaV2(List<int> idBebida)
        {
            return _tipoProductoRepository.ObtenerIdTipoProductosBebidaV2(idBebida);
        }

        public List<int> ObtenerIdTipoProductosComidaV2(List<int> idComida)
        {
            return _tipoProductoRepository.ObtenerIdTipoProductosComidaV2(idComida);
        }

    }
}
