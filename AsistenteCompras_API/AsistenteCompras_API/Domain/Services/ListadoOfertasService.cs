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

        public ListadoOfertasUsuario BuscarListado(int idListado, int idUsuario)
        {
            ListadoOfertasUsuario listadoAsociadoAlUsuario = _listadoOfertasRepository.BuscarListado(idListado, idUsuario);

            List<OfertaCantidadDTO> ofertasAsociadasAListado = new List<OfertaCantidadDTO>();

            if (listadoAsociadoAlUsuario!=null)
            {
                ofertasAsociadasAListado = _listadoOfertasRepository.BuscarOfertasAsociadas(listadoAsociadoAlUsuario.IdListado);
            }

            if (ofertasAsociadasAListado.Count != 0)
            {
                listadoAsociadoAlUsuario.Ofertas = ofertasAsociadasAListado;
            }
            return listadoAsociadoAlUsuario;
        }

        public int GuardarListadoConOfertas(ListadoOfertasDTO listadoOfertas)
        {
            //Inyecto a usuario un listado con sus ofertas asociadas y los tipos de comida y bebida que corresponden las ofertas
            int idListado = GuardarListado(listadoOfertas);
            GuardarOfertasEnListado(idListado, listadoOfertas);
            GuardarComidasElegidas(listadoOfertas.IdComidas, idListado);
            GuardarBebidasElegidas(listadoOfertas.IdBebidas, idListado);

            //Verifico si quedó guardado OK
            ListadoOfertasUsuario listadoGuardado = BuscarListado(idListado, listadoOfertas.IdUsuario);

            return listadoGuardado!=null ? listadoGuardado.IdListado : 0;
        }

        public List<ListadoDeOfertas> ConsultarListados()
        {
            throw new NotImplementedException();
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
            nuevoListado.IdEvento = listado.IdEvento;
            nuevoListado.CantidadOfertasElegidas = listado.Ofertas.Count();
            nuevoListado.Total = total;
            nuevoListado.Estado = true;
            nuevoListado.FechaCreacion = DateTime.Now;

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

        private void GuardarComidasElegidas(List<int> idComidas, int idListado)
        {
            foreach (int idComida in idComidas)
            {
                _listadoOfertasRepository.GuardarComida(new ListadoOfertasComida() { IdComida = idComida, IdListadoDeOfertas = idListado });
            }
        }

        private void GuardarBebidasElegidas(List<int> idBebidas, int idListado)
        {
            foreach (int idBebida in idBebidas)
            {
                _listadoOfertasRepository.GuardarBebida(new ListadoOfertasBebida() { IdBebida = idBebida, IdListadoDeOfertas = idListado });
            }
        }
    }
}
