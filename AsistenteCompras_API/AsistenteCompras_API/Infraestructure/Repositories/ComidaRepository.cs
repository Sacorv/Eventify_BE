using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Infraestructure.Contexts;
using AsistenteCompras_API.Domain.Services;

namespace AsistenteCompras_API.Infraestructure.Repositories;

public class ComidaRepository : IComidaRepository
{
    private readonly AsistenteComprasContext _context;

    public ComidaRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public int ObtenerCantidadMinimaDeComensales(int idComida)
    {
        return _context.Comida.Where(c => c.Id.Equals(idComida))
                              .Select(c => c.Comensales).FirstOrDefault();
    }

    public List<Comidum> ObtenerComida(int idComida)
    {
        return _context.Comida.Where(c => c.Id.Equals(idComida)).ToList();
    }

    public List<TipoProductoDTO> ObtenerIngredientes(int idComida)
    {
        return _context.ComidaTipoProductos.Where(c => c.IdComida.Equals(idComida))
                                           .Select(c => new TipoProductoDTO
                                           {
                                               Id = c.IdTipoProducto,
                                               Nombre = c.IdTipoProductoNavigation.Nombre,
                                               Peso = c.Gramos,
                                               Unidades = c.Unidades
                                           }).ToList();
    }
}
