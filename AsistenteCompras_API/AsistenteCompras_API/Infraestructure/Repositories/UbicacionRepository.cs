using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Contexts;

namespace AsistenteCompras_API.Infraestructure.Repositories;

public class UbicacionRepository : IUbicacionRepository
{
    private readonly AsistenteComprasContext _context;

    public UbicacionRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<LocalidadDTO> ObtenerTodasLasLocalidades()
    {
        return _context.Localidads.Select(l => new LocalidadDTO
        {
            Id = l.Id,
            Nombre = l.Nombre
        }).ToList();
    }

    public int BuscarLocalidadPorNombre(string localidad)
    {
        Localidad localidadBuscada = _context.Localidads.FirstOrDefault(l => l.Nombre.Equals(localidad));

        if(localidadBuscada != null)
        {
            return localidadBuscada.Id;
        }
        else
        {
            return 0;
        }
    }
}
