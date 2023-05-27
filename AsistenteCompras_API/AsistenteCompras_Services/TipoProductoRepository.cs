using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Repository;

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
}
