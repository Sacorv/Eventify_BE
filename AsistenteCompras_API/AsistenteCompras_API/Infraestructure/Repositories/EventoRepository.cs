using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Infraestructure.Contexts;
using AsistenteCompras_API.Domain.Services;

namespace AsistenteCompras_API.Infraestructure.Repositories;

public class EventoRepository : IEventoRepository
{

    private AsistenteComprasContext _context;

    public EventoRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<Bebidum> ObtenerBebidas(int idEvento)
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
        return _context.EventoComida
                                   .Where(ev => ev.IdEventoNavigation.Id == idEvento)
                                   .Select(ev => ev.IdComidaNavigation).ToList();
    }

    public List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida)
    {
        List<TipoProductoDTO> listado = new List<TipoProductoDTO>();

        List<TipoProductoDTO> productosParaComida = _context.EventoComida.Where(ec => ec.IdEventoNavigation.Id == idEvento && ec.IdComida == idComida)
                                                                         .Join(_context.ComidaTipoProductos, ctp => ctp.IdComida, ec => ec.IdComida, (ec, ctp)
                                                                          => new TipoProductoDTO
                                                                          {
                                                                              Id = ctp.IdTipoProductoNavigation.Id,
                                                                              Nombre = ctp.IdTipoProductoNavigation.Nombre
                                                                          }).ToList();

        List<TipoProductoDTO> productosParaBebida = _context.EventoBebida.Where(eb => eb.IdEventoNavigation.Id == idEvento && eb.IdBebida == idBebida)
                                                                         .Join(_context.BebidaTipoProductos, btp => btp.IdBebida, eb => eb.IdBebida, (eb, btp)
                                                                          => new TipoProductoDTO
                                                                          {
                                                                              Id = btp.IdTipoProductoNavigation.Id,
                                                                              Nombre = btp.IdTipoProductoNavigation.Nombre
                                                                          }).ToList();

        listado.AddRange(productosParaComida);
        listado.AddRange(productosParaBebida);

        return listado;
    }

    public List<TipoProductoDTO> ObtenerBebidaTipoProductos(int idBebida)
    {
        return _context.BebidaTipoProductos.Where(b => b.IdBebida == idBebida)
                                           .Select(b => new TipoProductoDTO
                                           {
                                               Id = b.IdTipoProducto,
                                               Nombre = b.IdTipoProductoNavigation.Nombre,
                                               Ingrediente = false,
                                               Peso = b.Mililitros,
                                               Unidades = b.Unidades
                                           })
                                           .ToList();
    }

    public int ObtenerCantidadMinimaBebidaPorInvitados(int idTipoProducto)
    {
        return _context.BebidaTipoProductos.Where(b => b.IdTipoProducto.Equals(idTipoProducto))
                                           .Select(b => b.Beben).First();
    }
}
