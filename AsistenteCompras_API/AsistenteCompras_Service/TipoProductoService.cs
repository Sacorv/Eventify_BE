using AsistenteCompras_Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
