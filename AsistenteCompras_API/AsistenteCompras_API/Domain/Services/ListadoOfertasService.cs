using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services
{
    public class ListadoOfertasService : IListadoOfertasService
    {
        private readonly IListadoOfertasRepository _listadoOfertasRepository;

        public ListadoOfertasService(IListadoOfertasRepository listadoOfertasRepository)
        {
            _listadoOfertasRepository = listadoOfertasRepository;
        }

        public ListadoDeOfertas BuscarListado(int idListado)
        {
            return _listadoOfertasRepository.BuscarListado(idListado);
        }

        public List<ListadoDeOfertas> ConsultarListados()
        {
            throw new NotImplementedException();
        }

        public int GuardarListadoConOfertas(ListadoOfertasDTO listadoOfertas)
        {
            int idListado = GuardarListado(listadoOfertas);

            GuardarOfertasEnListado(idListado, listadoOfertas);

            ListadoDeOfertas listadoGuardado = BuscarListado(idListado);

            return listadoGuardado.Id;
        }

        public void ModificarListado(ListadoDeOfertas listado)
        {
            throw new NotImplementedException();
        }


        private int GuardarListado(ListadoOfertasDTO listado)
        {
            ListadoDeOfertas nuevoListado = new ListadoDeOfertas();
            double total = 0;
            listado.Ofertas.ForEach(lo => total += lo.Subtotal);

            nuevoListado.IdUsuario = listado.IdUsuario;
            nuevoListado.CantidadOfertasElegidas = listado.Ofertas.Count();
            nuevoListado.Total = total;
            nuevoListado.Estado = true;

            int idListado = _listadoOfertasRepository.GuardarListado(nuevoListado);

            return idListado;
        }

        private void GuardarOfertasEnListado(int idListado ,ListadoOfertasDTO listadoOfertas)
        {
            foreach (OfertaElegidaDTO oferta in listadoOfertas.Ofertas)
            {
                OfertaElegida ofertaNueva = new OfertaElegida();

                ofertaNueva.NombreProducto = oferta.NombreProducto;
                ofertaNueva.IdPublicacion = oferta.IdPublicacion;
                ofertaNueva.Precio = oferta.Precio;
                ofertaNueva.Cantidad = oferta.Cantidad;
                ofertaNueva.Subtotal = oferta.Subtotal;
                ofertaNueva.IdListadoDeOfertas = idListado;
                ofertaNueva.Estado = true;

                _listadoOfertasRepository.GuardarOfertaEnListado(ofertaNueva);
            }
        }
    }
}
