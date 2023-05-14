using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            this._context = context;
        }

        public List<OfertaDTO> BuscarOfertasPorLocalidadYEvento(int idEvento, string localidad)
        {
            List<OfertaDTO> ofertas = new List<OfertaDTO>();
            try
            {
                var result = from EventoProducto ep in _context.EventoProductos
                             join Evento e in _context.Eventos on ep.IdEvento equals e.Id
                             join Producto p in _context.Productos on ep.IdProducto equals p.Id
                             join Publicacion pub in _context.Publicacions on p.Id equals pub.IdProducto
                             join Comercio c in _context.Comercios on pub.IdComercio equals c.Id
                             where c.Localidad == localidad && e.Id == idEvento
                             select new OfertaDTO
                             {
                                 NombreProducto = p.Nombre,
                                 Marca = p.Marca,
                                 Imagen = p.Imagen,
                                 Precio = pub.Precio,
                                 NombreComercio = c.RazonSocial
                             };

                foreach (OfertaDTO item in result)
                {
                    ofertas.Add(item);
                }
            }
            catch (DbException e)
            {
                throw new Exception(e.Message);
            }
            return ofertas;
        }

    }
}
