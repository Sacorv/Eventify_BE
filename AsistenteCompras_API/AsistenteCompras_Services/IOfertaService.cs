using AsistenteCompras_Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public interface IOfertaService
    {
        List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento);
        
        List<OfertaDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad);

    }
}
