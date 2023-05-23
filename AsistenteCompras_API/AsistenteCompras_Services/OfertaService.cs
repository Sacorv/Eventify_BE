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

    public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida)
    {
        List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();
        List<int> idTiposProducto = new List<int>();

        //ObtengoLosProductoParaLaComida
        var productosComida = _context.ComidaTipoProductos.Where(c => c.IdComida == idComida)
                                                      .Select(c => c.IdTipoProducto).ToList();
        //Bebida
        var productosBebida = _context.BebidaTipoProductos.Where(b => b.IdBebida == idBebida)
                                                          .Select(b => b.IdTipoProducto).ToList();

        idTiposProducto.AddRange(productosComida);
        idTiposProducto.AddRange(productosBebida);


        //Recorrer cada producto que se necesita
        foreach(var idTipoProducto in idTiposProducto)
        {
            try
            {
                OfertaDTO recomendacion = new OfertaDTO();

                //ObtenerPrecioMinimo
                Decimal precioMinimo = _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto).Min(p => p.Precio);

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
                if(ofertas.Count > 1)
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
                
                listaCompraEconomica.Add(recomendacion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
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
