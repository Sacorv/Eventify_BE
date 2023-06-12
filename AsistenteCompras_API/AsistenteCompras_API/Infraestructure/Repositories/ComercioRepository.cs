using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AsistenteCompras_API.Infraestructure.Repositories;

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

    public string ObtenerImagenComercio(int idComercio)
    {
        return _context.Comercios.Where(c => c.Id == idComercio)
                                 .Select(c => c.Imagen)
                                 .First();
    }
}
