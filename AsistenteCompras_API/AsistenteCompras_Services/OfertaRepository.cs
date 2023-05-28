using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AsistenteCompras_Repository;

public class OfertaRepository : IOfertaRepository
{

    private AsistenteComprasContext _context;

    public OfertaRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, decimal precio)
    {
        return _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && p.Precio == precio)
                                    .Select(o => new OfertaDTO
                                    {
                                        IdPublicacion = o.Id,
                                        IdTipoProducto = o.IdProductoNavigation.IdTipoProducto,
                                        IdLocalidad = o.IdComercioNavigation.IdLocalidad,
                                        NombreProducto = o.IdProductoNavigation.Nombre,
                                        Marca = o.IdProductoNavigation.Marca,
                                        Imagen = o.IdProductoNavigation.Imagen,
                                        Precio = o.Precio,
                                        NombreComercio = o.IdComercioNavigation.RazonSocial,
                                        Latitud = o.IdComercioNavigation.Latitud,
                                        Longitud = o.IdComercioNavigation.Longitud,
                                        Localidad = o.IdComercioNavigation.IdLocalidadNavigation.Nombre
                                    }).ToList();
    }

    public decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto)
    {
        return _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && localidades.Contains(p.IdComercioNavigation.IdLocalidad))
                                    .Min(p => p.Precio);
    }

    public List<OfertaDTO> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida)
    {
        List<OfertaDTO> publicaciones = new List<OfertaDTO>();

        var idProductosParaComida = _context.ComidaTipoProductos
                                    .Where(ctp => ctp.IdComida == idComida)
                                    .Select(ctp => ctp.IdTipoProductoNavigation.Id);

        var idProductosParaBebida = _context.BebidaTipoProductos
                                    .Where(btp => btp.IdBebida == idBebida)
                                    .Select(btp => btp.IdTipoProductoNavigation.Id);

            publicaciones = _context.Publicacions.Where(pub => pub.IdComercioNavigation.IdLocalidadNavigation.Id == idLocalidad)
                                                 .Join(_context.Productos, pub => pub.IdProducto, p => p.Id, 
                                                       (pub, p) => new OfertaDTO
                                                       {
                                                           IdPublicacion = pub.Id,
                                                           IdTipoProducto = p.IdTipoProducto,
                                                           IdLocalidad = pub.IdComercioNavigation.IdLocalidad,
                                                           NombreProducto = p.Nombre,
                                                           Marca = p.Marca,
                                                           Imagen = p.Imagen,
                                                           Precio = pub.Precio,
                                                           NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                                           Latitud = pub.IdComercioNavigation.Latitud,
                                                           Longitud = pub.IdComercioNavigation.Longitud,
                                                           Localidad = pub.IdComercioNavigation.IdLocalidadNavigation.Nombre
                                                       })
                                                 .Where(oferta => idProductosParaComida.Contains(oferta.IdTipoProducto) || idProductosParaBebida.Contains(oferta.IdTipoProducto)).ToList();
        return publicaciones;
    }

    public List<Comercio> ComerciosDentroDelRadio(double latitud, double longitud, float distancia)
    {
        var comercios = _context.Comercios
                                .FromSqlInterpolated($"EXEC BuscarComerciosPorRadio {latitud}, {longitud}, {distancia}")
                                .ToList();
        return comercios;
    }

    public List<OfertaDTO> OfertasDentroDelRadio(int idComida, int idBebida, ArrayList idComercios)
    {
        List<OfertaDTO> publicaciones = new List<OfertaDTO>();

        var idProductosParaComida = _context.ComidaTipoProductos
                                    .Where(ctp => ctp.IdComida == idComida)
                                    .Select(ctp => ctp.IdTipoProductoNavigation.Id);

        var idProductosParaBebida = _context.BebidaTipoProductos
                                    .Where(btp => btp.IdBebida == idBebida)
                                    .Select(btp => btp.IdTipoProductoNavigation.Id);



        publicaciones = _context.Publicacions.Where(pub => idComercios.Contains(pub.IdComercio))
                            .Join(_context.Productos, pub => pub.IdProducto, p => p.Id, 
                                  (pub, p) => new OfertaDTO
                                  {
                                      IdPublicacion = pub.Id,
                                      IdTipoProducto = p.IdTipoProducto,
                                      IdLocalidad = pub.IdComercioNavigation.IdLocalidad,
                                      NombreProducto = p.Nombre,
                                      Marca = p.Marca,
                                      Imagen = p.Imagen,
                                      Precio = pub.Precio,
                                      NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                      Latitud = pub.IdComercioNavigation.Latitud,
                                      Longitud = pub.IdComercioNavigation.Longitud,
                                      Localidad = pub.IdComercioNavigation.IdLocalidadNavigation.Nombre
                                  })
                            .Where(oferta => idProductosParaComida.Contains(oferta.IdTipoProducto) || idProductosParaBebida.Contains(oferta.IdTipoProducto)).ToList();


        return publicaciones;
    }
}
