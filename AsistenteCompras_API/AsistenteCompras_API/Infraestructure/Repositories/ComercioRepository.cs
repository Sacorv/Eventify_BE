using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;
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

    public Login VerificarComercio(string email, string clave)
    {
        return _context.Comercios.Where(c => c.Email
                                    .Equals(email) && c.Clave.Equals(clave))
                                    .Select(c => new Login
                                    {
                                        Id = c.Id,
                                        Rol = c.IdRolNavigation.Nombre,
                                        Usuario = c.RazonSocial + " " + c.CUIT,
                                        Email = c.Email
                                    }).FirstOrDefault()!;
    }

    public Comercio RegistrarComercio(Comercio comercio)
    {
        Comercio verificarEmail = _context.Comercios.FirstOrDefault(c => c.Email.Equals(comercio.Email))!;

        Comercio verificarCuit = _context.Comercios.FirstOrDefault(c => c.CUIT.Equals(comercio.CUIT))!;

        if (verificarEmail==null && verificarCuit==null)
        {
            _context.Comercios.Add(comercio);
            _context.SaveChanges();

            return comercio;
        }
        else
        {
            return null!;
        }
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
