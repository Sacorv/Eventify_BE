using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using Microsoft.Spatial;
using NetTopologySuiteOperationDistance = NetTopologySuite.Operation.Distance;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Point = NetTopologySuite.Geometries.Point;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using NetTopologySuite.IO;
using Geometry = NetTopologySuite.Geometries.Geometry;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections;

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
                Decimal precioMinimo = _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && localidades.Contains(p.IdComercioNavigation.IdLocalidad))
                                                            .Min(p => p.Precio);

                //
                 List<OfertaDTO> ofertas = _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && p.Precio == precioMinimo)
                                  .Select(o => new OfertaDTO
                                  {
                                      NombreProducto = o.IdProductoNavigation.Nombre,
                                      Marca = o.IdProductoNavigation.Marca,
                                      Imagen = o.IdProductoNavigation.Imagen,
                                      Precio = o.Precio,
                                      NombreComercio = o.IdComercioNavigation.RazonSocial,
                                      Latitud = o.IdComercioNavigation.Latitud,
                                      Longitud = o.IdComercioNavigation.Longitud,
                                      Localidad = o.IdComercioNavigation.IdLocalidadNavigation.Nombre
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


    public List<OfertasDTO> OfertasParaEventoPorLocalidad(int idLocalidad, int idComida, int idBebida)
    {
        List<OfertasDTO> publicaciones = new List<OfertasDTO>();

        var idProductosParaComida = _context.ComidaTipoProductos
                                    .Where(ctp => ctp.IdComida == idComida)
                                    .Select(ctp => ctp.IdTipoProductoNavigation.Id);

        var idProductosParaBebida = _context.BebidaTipoProductos
                                    .Where(btp => btp.IdBebida == idBebida)
                                    .Select(btp => btp.IdTipoProductoNavigation.Id);



        publicaciones = _context.Publicacions.Where(pub => pub.IdComercioNavigation.IdLocalidad == idLocalidad)
                            .Join(_context.Productos, pub => pub.IdProducto, p => p.Id, (pub, p)
                                => new OfertasDTO
                                {
                                    IdPublicacion = pub.Id,
                                    IdTipoProducto = p.IdTipoProducto,
                                    NombreProducto = p.Nombre,
                                    Marca = p.Marca,
                                    Imagen = p.Imagen,
                                    Precio = pub.Precio,
                                    NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                    Latitud = pub.IdComercioNavigation.Latitud,
                                    Longitud = pub.IdComercioNavigation.Longitud
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


    public List<OfertasDTO> OfertasDentroDelRadio(int idComida, int idBebida, ArrayList idComercios)
    {
        List<OfertasDTO> publicaciones = new List<OfertasDTO>();

        var idProductosParaComida = _context.ComidaTipoProductos
                                    .Where(ctp => ctp.IdComida == idComida)
                                    .Select(ctp => ctp.IdTipoProductoNavigation.Id);

        var idProductosParaBebida = _context.BebidaTipoProductos
                                    .Where(btp => btp.IdBebida == idBebida)
                                    .Select(btp => btp.IdTipoProductoNavigation.Id);



        publicaciones = _context.Publicacions.Where(pub => idComercios.Contains(pub.IdComercio))
                            .Join(_context.Productos, pub => pub.IdProducto, p => p.Id, (pub, p)
                                => new OfertasDTO
                                {
                                    IdPublicacion = pub.Id,
                                    IdTipoProducto = p.IdTipoProducto,
                                    NombreProducto = p.Nombre,
                                    Marca = p.Marca,
                                    Imagen = p.Imagen,
                                    Precio = pub.Precio,
                                    NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                    Latitud = pub.IdComercioNavigation.Latitud,
                                    Longitud = pub.IdComercioNavigation.Longitud
                                })
                            .Where(oferta => idProductosParaComida.Contains(oferta.IdTipoProducto) || idProductosParaBebida.Contains(oferta.IdTipoProducto)).ToList();


        return publicaciones;
    }
}
