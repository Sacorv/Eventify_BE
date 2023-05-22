using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services;

public class OfertaService : IOfertaService
{

    private AsistenteComprasContext _context;

    public OfertaService(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida)
    {
        List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();
        List<int> idTiposProducto = new List<int>();

        //ObtengoLosProductoParaLaComida
        var productosComida = _context.ComidaTipoProductos.Where(c => c.IdComida == idComida)
                                                      .Select(c => c.IdTipoProducto).ToList();

        var productosBebida = _context.BebidaTipoProductos.Where(b => b.IdBebida == idBebida)
                                                          .Select(b => b.IdTipoProducto).ToList();

        idTiposProducto.AddRange(productosComida);
        idTiposProducto.AddRange(productosBebida);


        //Recorrer cada producto que se necesita
        foreach(var idTipoProducto in idTiposProducto)
        {
            OfertaDTO recomendacion = new OfertaDTO();

            try
            {
                //ObtenerPrecioMinimo
                var precioMinimo = _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto).Min(p => p.Precio);

                //
                 List<OfertaDTO> ofertas = _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && p.Precio == precioMinimo)
                                  .Select(o => new OfertaDTO
                                  {
                                      NombreProducto = o.IdProductoNavigation.Nombre,
                                      Marca = o.IdProductoNavigation.Marca,
                                      Imagen = o.IdProductoNavigation.Imagen,
                                      Precio = o.Precio,
                                      NombreComercio = o.IdComercioNavigation.RazonSocial
                                  })
                                  .ToList();

                //ElegirUnaOferta
                if(ofertas.Count() > 1)
                {
                    //ElegirPorLocalidadPreferencia
                    int buscando = 0;
                    foreach(int idLocalidad in localidades)
                    {
                        foreach(OfertaDTO oferta in ofertas)
                        {
                             buscando = _context.Publicacions
                                .Where(p => p.IdComercioNavigation.RazonSocial == oferta.NombreComercio &&
                                p.IdProductoNavigation.Nombre == oferta.NombreProducto &&
                                p.IdComercioNavigation.IdLocalidad == idLocalidad).Count();

                            if(buscando > 0)
                            {
                                recomendacion = oferta;
                                break;
                            }
                        }

                        if (buscando > 0) break;
                    }
                }
                else
                {
                    recomendacion = ofertas.First();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            listaCompraEconomica.Add(recomendacion);
        }
        return listaCompraEconomica;
    }
    
    public List<OfertaDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad)
    {
        List<OfertaDTO> ofertas = new List<OfertaDTO>();
        //try
        //{
        //    var result = from EventoProducto ep in _context.EventoProductos
        //                 join Evento e in _context.Eventos on ep.IdEvento equals e.Id
        //                 join Producto p in _context.Productos on ep.IdProducto equals p.Id
        //                 join Publicacion pub in _context.Publicacions on p.Id equals pub.IdProducto
        //                 join Comercio c in _context.Comercios on pub.IdComercio equals c.Id
        //                 where c.Localidad == localidad && e.Id == idEvento
        //                 select new OfertaDTO
        //                 {
        //                     NombreProducto = p.Nombre,
        //                     Marca = p.Marca,
        //                     Imagen = p.Imagen,
        //                     Precio = pub.Precio,
        //                     NombreComercio = c.RazonSocial
        //                 };

        //    foreach (OfertaDTO item in result)
        //    {
        //        ofertas.Add(item);
        //    }
        //}
        //catch (DbException e)
        //{
        //    throw new Exception(e.Message);
        //}
        return ofertas;
    }

}
