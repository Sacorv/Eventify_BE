using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using Microsoft.Spatial;
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

        List<OfertasDTO> ObtenerOfertasPorZonaGeografica(decimal latitud, decimal longitud, float distancia, int idComida, int idBebida);
    }
}
