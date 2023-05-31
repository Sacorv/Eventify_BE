using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AsistenteCompras_Infraestructure.Repositories
{
    public class ComercioRepository : IComercioRepository
    {
        private AsistenteComprasContext _context;

        public ComercioRepository(AsistenteComprasContext context)
        {
            _context = context;
        }

        public List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia)
        {
            return _context.Comercios
                                .FromSqlInterpolated($"EXEC BuscarComerciosPorRadio {latitud}, {longitud}, {distancia}")
                                .ToList();
        }

    }
}
