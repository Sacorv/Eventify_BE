using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Repository;

namespace AsistenteCompras_Services;

public class OfertaService : IOfertaService
{
    private IOfertaRepository _repositoryOferta;

    private ITipoProductoRepository _repositoryTipoProducto;

    public OfertaService(IOfertaRepository repositoryOferta, ITipoProductoRepository repositoryTipoProducto)
    {
        _repositoryOferta = repositoryOferta;
        _repositoryTipoProducto = repositoryTipoProducto;
    }


    public List<OfertaDTO> ObtenerOfertasMenorPrecioPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida)
    {
        List<OfertaDTO> ofertas = _repositoryOferta.OfertasParaEventoPorLocalidad(idLocalidad, idComida, idBebida);
       
        return ListarOfertasBaratas(ofertas);
    }


    private List<OfertaDTO> ListarOfertasBaratas(List<OfertaDTO> ofertas)
    {
        List<OfertaDTO> ofertasBaratas = new List<OfertaDTO>();

        int idMaximo = IdProductoMaximo(ofertas);

        do
        {
            OfertaDTO masBarata = new OfertaDTO();
            masBarata.Precio = 999999999999999999;

            for (int i = 0; i <= ofertas.Count() - 1; i++)
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

    private int IdProductoMaximo(List<OfertaDTO> ofertas)
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

    public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida)
    {
        List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();
        List<int> idTiposProducto = new List<int>();

        List<int> productosComida = _repositoryTipoProducto.ObtenerIdTipoProductosComida(idComida);
        List<int> productosBebida = _repositoryTipoProducto.ObtenerIdTipoProductosBebida(idBebida);

        idTiposProducto.AddRange(productosComida);
        idTiposProducto.AddRange(productosBebida);

        //Recorrer cada producto que se necesita
        foreach (var idTipoProducto in idTiposProducto)
        {
            try
            {
                Decimal precioMinimo = _repositoryOferta.ObtenerPrecioMinimoDelProductoPorLocalidad(localidades, idTipoProducto);

                List<OfertaDTO> ofertas = _repositoryOferta.ObtenerOfertasPorPrecio(idTipoProducto, precioMinimo);

                if(ofertas.Count > 1)
                {
                    listaCompraEconomica.Add(SeleccionarOferta(localidades, ofertas));
                }
                else
                {
                    listaCompraEconomica.Add(ofertas.First());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return listaCompraEconomica;
    }

    private static OfertaDTO SeleccionarOferta(List<int> localidades, List<OfertaDTO> ofertas)
    {
        OfertaDTO recomendacion = new OfertaDTO();

        foreach (int idLocalidad in localidades)
        {
            var resultado = ofertas.Find(o => o.IdLocalidad == idLocalidad);
            if (resultado != null)
            {
                recomendacion = resultado;
                break;
            }
        }
        return recomendacion;
    }
}
