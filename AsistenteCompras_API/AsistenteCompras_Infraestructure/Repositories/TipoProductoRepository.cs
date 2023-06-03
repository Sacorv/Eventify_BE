using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Infraestructure.Contexts;

namespace AsistenteCompras_Infraestructure.Repositories;

public class TipoProductoRepository : ITipoProductoRepository
{
    private AsistenteComprasContext _context;

    public TipoProductoRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<int> ObtenerIdTipoProductosBebida(int idBebida)
    {
        return _context.BebidaTipoProductos.Where(b => b.IdBebida == idBebida)
                                           .Select(b => b.IdTipoProducto).ToList();
    }

    public List<int> ObtenerIdTipoProductosComida(int idComida)
    {
        return _context.ComidaTipoProductos.Where(c => c.IdComida == idComida)
                                           .Select(c => c.IdTipoProducto).ToList();
    }

    public List<int> ObtenerIdTipoProductosBebidaV2(List<int> idBebida)
    {
        return _context.BebidaTipoProductos.Where(b => idBebida.Contains(b.IdBebida))
                                           .Select(b => b.IdTipoProducto).ToList();
    }

    public List<int> ObtenerIdTipoProductosComidaV2(List<int> idComida)
    {
        return _context.ComidaTipoProductos.Where(c => idComida.Contains(c.IdComida))
                                           .Select(c => c.IdTipoProducto).ToList();
    }
}
