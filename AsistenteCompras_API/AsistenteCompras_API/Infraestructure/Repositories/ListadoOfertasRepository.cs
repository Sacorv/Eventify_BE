using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Contexts;

namespace AsistenteCompras_API.Infraestructure.Repositories
{
    public class ListadoOfertasRepository : IListadoOfertasRepository
    {
        private readonly AsistenteComprasContext _context;

        public ListadoOfertasRepository(AsistenteComprasContext context)
        {
            _context = context;
        }

        public ListadoOfertasUsuario BuscarListado(int idListado, int idUsuario)
        {
            List<string> comidasElegidas = BuscarComidasElegidasPorIdListado(idListado);
            List<string> bebidasElegidas = BuscarBebidasElegidasPorIdListado(idListado);

            ListadoOfertasUsuario listado = _context.ListadoDeOfertas.Where(lo => lo.Id == idListado).Join(_context.Usuarios, lo => lo.IdUsuario, u => u.Id, (lo, u)
                                                                        => new ListadoOfertasUsuario
                                                                        {
                                                                            IdListado = lo.Id,
                                                                            IdUsuario = u.Id,
                                                                            Usuario = u.Nombre + " " + u.Apellido,
                                                                            FechaCreacion = lo.FechaCreacion.ToString("dd-MM-yy"),
                                                                            Evento = lo.IdEventoNavigation.Nombre,
                                                                            ComidasElegidas = comidasElegidas,
                                                                            BebidasElegidas = bebidasElegidas,
                                                                            CantidadOfertas = lo.CantidadOfertasElegidas,
                                                                            TotalListado = lo.Total,
                                                                            UrlRecorrido = lo.UrlRecorrido,
                                                                            MensajeOfertas = lo.MensajeOfertas,
                                                                            DistanciaARecorrer = lo.DistanciaARecorrer
                                                                        }).Where(listadoOfertas => listadoOfertas.IdUsuario == idUsuario)
                                                                        .FirstOrDefault()!;
            return listado;
        }

        public List<OfertaCantidadDTO> BuscarOfertasAsociadas(int idListado)
        {
            List<OfertaCantidadDTO> ofertas = _context.OfertaElegida.Where(oe => oe.IdListadoDeOfertas == idListado)
                        .Join(_context.Publicacions, oe => oe.IdListadoDeOfertas, pub => pub.Id, (oe, pub)
                         => new OfertaCantidadDTO
                         {
                            Cantidad = oe.Cantidad,
                            Oferta = new OfertaDTO
                            {
                                IdPublicacion = oe.IdPublicacion,
                                IdTipoProducto = oe.IdPublicacionNavigation.IdProductoNavigation.IdTipoProducto,
                                TipoProducto = oe.NombreProducto!,
                                NombreProducto = oe.IdPublicacionNavigation.IdProductoNavigation.Nombre,
                                Marca = oe.IdPublicacionNavigation.IdProductoNavigation.Marca,
                                Imagen = oe.IdPublicacionNavigation.IdProductoNavigation.Imagen,
                                Peso = oe.IdPublicacionNavigation.IdProductoNavigation.Peso,
                                Unidades = oe.IdPublicacionNavigation.IdProductoNavigation.Unidades,
                                Precio = oe.Precio,
                                NombreComercio = oe.IdPublicacionNavigation.IdComercioNavigation.RazonSocial,
                                Localidad = oe.IdPublicacionNavigation.IdComercioNavigation.IdLocalidadNavigation.Nombre,
                                IdLocalidad = oe.IdPublicacionNavigation.IdComercioNavigation.IdLocalidad,
                                Latitud = (double)oe.IdPublicacionNavigation.IdComercioNavigation.Latitud,
                                Longitud = (double)oe.IdPublicacionNavigation.IdComercioNavigation.Longitud,
                                FechaVencimiento = oe.IdPublicacionNavigation.FechaFin.ToString("dd-MM-yy")
                            },
                            Subtotal = oe.Subtotal
                         }).ToList();
            return ofertas;
        }

        public int GuardarListado(ListadoDeOfertas listado)
        {
            _context.ListadoDeOfertas.Add(listado);
            _context.SaveChanges();

            return listado.Id;
        }

        public void GuardarOfertaEnListado(OfertaElegida oferta)
        {
            _context.OfertaElegida.Add(oferta);
            _context.SaveChanges();
        }

        public void GuardarComida(ListadoOfertasComida comidaAsociada)
        {
            _context.ListadoOfertasComida.Add(comidaAsociada);
            _context.SaveChanges();
        }

        public void GuardarBebida(ListadoOfertasBebida bebidaAsociada)
        {
            _context.ListadoOfertasBebida.Add(bebidaAsociada);
            _context.SaveChanges();
        }


        public void ModificarListado(ListadoDeOfertas listado)
        {
            throw new NotImplementedException();
        }

        public List<ListadosUsuario> ObtenerListados(int idUsuario)
        {
            List<ListadosUsuario> listados = _context.ListadoDeOfertas.Where(lo => lo.IdUsuario == idUsuario).Join(_context.Usuarios, lo => lo.IdUsuario, u => u.Id, (lo, u)
                                                                        => new ListadosUsuario
                                                                        {
                                                                            IdListado = lo.Id,
                                                                            IdUsuario = u.Id,
                                                                            FechaCreacion = lo.FechaCreacion.ToString("dd-MM-yy"),
                                                                            Evento = lo.IdEventoNavigation.Nombre,
                                                                            CantidadOfertas = lo.CantidadOfertasElegidas,
                                                                            TotalListado = lo.Total
                                                                        }).ToList();

            foreach(ListadosUsuario listado in listados){
                listado.ComidasElegidas = BuscarComidasElegidasPorIdListado(listado.IdListado);
                listado.BebidasElegidas = BuscarBebidasElegidasPorIdListado(listado.IdListado);
            }

            return listados;
        }

        private List<string> BuscarComidasElegidasPorIdListado(int idListado)
        {
            return _context.ListadoOfertasComida.Where(loc => loc.IdListadoDeOfertas == idListado)
                                                .Select(loc => loc.IdComidaNavigation.Nombre).ToList();
                    
        }

        private List<string> BuscarBebidasElegidasPorIdListado(int idListado)
        {
            return _context.ListadoOfertasBebida.Where(loc => loc.IdListadoDeOfertas == idListado)
                                                .Select(loc => loc.IdBebidaNavigation.TipoBebida).ToList();

        }

    }
}
