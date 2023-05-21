using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services;

public class UbicacionService : IUbicacionService
{
    private readonly AsistenteComprasContext _context;

    public UbicacionService(AsistenteComprasContext context)
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
}
