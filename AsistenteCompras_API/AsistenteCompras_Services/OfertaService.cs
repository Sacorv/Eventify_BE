using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AsistenteCompras_Services;

public class OfertaService : IOfertaService
{

    private AsistenteComprasContext _context;

    public OfertaService(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento)
    {
        List<Producto> productosEvento;
        List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();
        
        //productosEvento = _context.EventoProductos
        //                          .Where(evento => evento.IdEvento == idEvento)
        //                          .Select(evento => evento.IdProductoNavigation)
        //                          .ToList();

        //foreach (Producto producto in productosEvento)
        //{   
        //    OfertaDTO productoBarato = _context.Publicacions
        //                                       .Where(p => p.IdProducto == producto.Id)
        //                                       .OrderBy(p => p.Precio)
        //                                       .Select(o => new OfertaDTO
        //                                       {
        //                                        NombreProducto = o.IdProductoNavigation.Nombre,
        //                                        Imagen = o.IdProductoNavigation.Imagen,
        //                                        Marca = o.IdProductoNavigation.Marca,
        //                                        NombreComercio = o.IdComercioNavigation.RazonSocial,
        //                                        Precio = o.Precio,
        //                                       }).First();

        //    listaCompraEconomica.Add(productoBarato);
        //}

        return listaCompraEconomica;
    }


    public List<OfertaDTOPrueba> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida)
    {
        List<OfertaDTOPrueba> publicaciones = new List<OfertaDTOPrueba>();

        var idProductosParaComida = _context.ComidaTipoProductos
                                    .Where(ctp => ctp.IdComida == idComida)
                                    .Select(ctp => ctp.IdTipoProductoNavigation.Id);

        var idProductosParaBebida = _context.BebidaTipoProductos
                                    .Where(btp => btp.IdBebida == idBebida)
                                    .Select(btp => btp.IdTipoProductoNavigation.Id);


        foreach(int idProducto in idProductosParaComida)
        {

            publicaciones = _context.Publicacions.Where(pub => pub.IdComercioNavigation.IdLocalidadNavigation.Id == idLocalidad)
                                .Join(_context.Productos, pub => pub.IdProducto, p => p.Id, (pub, p)
                                 => new OfertaDTOPrueba { IdPublicacion=pub.Id, IdTipoProducto = p.IdTipoProducto, NombreProducto = p.Nombre, 
                                                          Marca = p.Marca, Imagen = p.Imagen, Precio = pub.Precio, NombreComercio=pub.IdComercioNavigation.RazonSocial })
                                .Where(oferta => idProductosParaComida.Contains(oferta.IdTipoProducto) || idProductosParaBebida.Contains(oferta.IdTipoProducto)).ToList();
        }

        return publicaciones;
    }
}
