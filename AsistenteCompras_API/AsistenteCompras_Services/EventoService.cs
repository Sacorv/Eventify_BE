using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public class EventoService : IEventoService
    {

        public AsistenteComprasContext _context;

        public EventoService(AsistenteComprasContext context)
        {
            this._context = context;
        }

        public List<PublicacionDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad)
        {
            
            List<PublicacionDTO> ofertas = new List<PublicacionDTO>();

            var result = from EventoProducto ep in _context.EventoProductos
                         join Evento e in _context.Eventos on ep.IdEvento equals e.Id
                         join Producto p in _context.Productos on ep.IdProducto equals p.Id
                         join Publicacion pub in _context.Publicacions on p.Id equals pub.IdProducto
                         join Comercio c in _context.Comercios on pub.IdComercio equals c.Id
                         where c.Localidad == localidad && e.Id == idEvento
                         select new PublicacionDTO
                         {
                             NombreProducto = p.Nombre,
                             Marca = p.Marca,
                             Imagen = p.Imagen,
                             Precio = pub.Precio,
                             NombreComercio = c.RazonSocial                            
                         };

            foreach (PublicacionDTO item in result)
            {
                ofertas.Add(item);
            }

            return ofertas;
      
        }

        public List<Evento> ObtenerEventos()
        {

            return this._context.Eventos.ToList();

        }
    }
}
