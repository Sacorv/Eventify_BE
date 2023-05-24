using AsistenteCompras_Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Service
{
    public interface IOfertaServicio
    {

        List<OfertasDTO> ObtenerOfertasMenorPrecioPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida);


    }
}
