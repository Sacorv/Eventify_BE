using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public interface ITipoProductoService
    {
        List<int> ObtenerIdTipoProductosBebida(int idBebida);

        List<int>  ObtenerIdTipoProductosComida(int idComida);
    }
}
