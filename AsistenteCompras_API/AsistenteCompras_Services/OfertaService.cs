using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public class OfertaService : IOfertaService
    {
        private AsistenteComprasContext _context;

        public OfertaService(AsistenteComprasContext context)
        {
            _context = context;
        }
    
        public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idEvento)
        {
            Publicacion publicacion = new Publicacion();
            List<Producto> productosEvento;
            List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();
            OfertaDTO productoBarato = new OfertaDTO();   

            //obtengo productos del evento
            productosEvento = _context.EventoProductos
                                      .Where(evento => evento.IdEvento == idEvento)
                                      .Select(evento => evento.IdProductoNavigation)
                                      .ToList();



            foreach (Producto producto in productosEvento)
            {
                publicacion = _context.Publicacions
                                      .Where(publicacion => publicacion.IdProducto == producto.Id)
                                      .MinBy(publicacion => publicacion.Precio);


                productoBarato.NombreProducto = producto.Nombre;
                productoBarato.Imagen = producto.Imagen;
                productoBarato.Marca = producto.Marca;
                productoBarato.NombreComercio = publicacion.IdComercioNavigation.RazonSocial;
                productoBarato.Precio = publicacion.Precio;

                listaCompraEconomica.Add(productoBarato);    
            }

            return listaCompraEconomica;
        }
    }
}
