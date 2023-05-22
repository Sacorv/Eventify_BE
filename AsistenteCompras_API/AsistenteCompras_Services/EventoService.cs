using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace AsistenteCompras_Services;

public class EventoService : IEventoService
{

    private AsistenteComprasContext _context;

    public EventoService(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<Bebidum> ObtenerBebidasPosibles(int idEvento)
    {
        return _context.EventoBebida.Where(e => e.IdEvento.Equals(idEvento))
                                    .Select(b => new Bebidum
                                    {
                                        Id = b.IdBebida,
                                        TipoBebida = b.IdBebidaNavigation.TipoBebida
                                    }).ToList();
    }

    public List<Evento> ObtenerEventos()
    {
        return _context.Eventos.ToList();
    }

    public List<Comidum> ObtenerComidas(int idEvento)
    {
        List<Comidum> comidas = new List<Comidum>();
        
        var comidaEvento = _context.EventoComida
                                   .Where(ev => ev.IdEventoNavigation.Id == idEvento)
                                   .Select(ev => ev.IdComidaNavigation).ToList();

            foreach (Comidum comida in comidaEvento)
            {
                comidas.Add(comida);
            }

            return comidas;
        }

    public List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida)
    {
        List<TipoProductoDTO> productos = new List<TipoProductoDTO>();

        var productosComida = from Evento e in _context.Eventos
                            join EventoComidum ec in _context.EventoComida on e.Id equals ec.IdEvento
                            join Comidum c in _context.Comida on ec.IdComida equals c.Id
                            join ComidaTipoProducto ctp in _context.ComidaTipoProductos on c.Id equals ctp.IdComida
                            join TipoProducto tp in _context.TipoProductos on ctp.IdTipoProducto equals tp.Id
                            where c.Id == idComida && e.Id == idEvento
                            select new TipoProductoDTO { Id=tp.Id, Nombre=tp.Nombre };

        var productosBebida = from Evento e in _context.Eventos
                            join EventoBebidum eb in _context.EventoBebida on e.Id equals eb.IdEvento
                            join Bebidum b in _context.Bebida on eb.IdBebida equals b.Id
                            join BebidaTipoProducto btp in _context.BebidaTipoProductos on b.Id equals btp.IdBebida
                            join TipoProducto tp in _context.TipoProductos on btp.IdTipoProducto equals tp.Id
                            where b.Id == idBebida && e.Id == idEvento
                            select new TipoProductoDTO { Id = tp.Id, Nombre = tp.Nombre };


        foreach (TipoProductoDTO prod in productosComida)
        {
            productos.Add(prod);
        }

        foreach (TipoProductoDTO prod in productosBebida)
        {
            productos.Add(prod);
        }

        return productos;
    }
}
