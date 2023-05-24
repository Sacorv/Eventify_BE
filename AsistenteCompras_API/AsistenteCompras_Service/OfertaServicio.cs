using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Service
{
    public class OfertaServicio : IOfertaServicio
    {
        //TO DO: cambiar nombre de ofertaService a OfertaRepository. Esto con todo el paquete AsistenteCompras_Services
        //TO DO: mover toda la lógica a este namespace

        private IOfertaService _repository;

        public OfertaServicio(IOfertaService repository)
        {
            _repository = repository;
        }


        public List<OfertasDTO> ObtenerOfertasMenorPrecioPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida)
        {
            List<OfertasDTO> ofertas = _repository.OfertasParaEventoPorLocalidad(idLocalidad, idComida, idBebida);
           
            return ListarOfertasBaratas(ofertas);
        }


        private List<OfertasDTO> ListarOfertasBaratas(List<OfertasDTO> ofertas)
        {
            List<OfertasDTO> ofertasBaratas = new List<OfertasDTO>();

            int idMaximo = IdProductoMaximo(ofertas);

            do
            {
                OfertasDTO masBarata = new OfertasDTO();
                masBarata.Precio = 999999999999999999;

                for (int i = 0; i < ofertas.Count() - 1; i++)
                {
                    if (ofertas[i].IdTipoProducto == idMaximo && ofertas[i].Precio < masBarata.Precio)
                    {
                        masBarata = ofertas[i];
                    }
                }
                idMaximo--;

                if (masBarata.IdTipoProducto != 0)
                {
                    ofertasBaratas.Add(masBarata);
                }

            } while (idMaximo > 0);

            return ofertasBaratas;
        }

        private int IdProductoMaximo(List<OfertasDTO> ofertas)
        {
            int idProdMáximo = 0;

            foreach (var item in ofertas)
            {
                if (item.IdTipoProducto > idProdMáximo)
                {
                    idProdMáximo = item.IdTipoProducto;
                }
            }
            return idProdMáximo;
        }
    }
}
