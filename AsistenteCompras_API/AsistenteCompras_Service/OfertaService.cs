using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Repository;
using System.Collections;

namespace AsistenteCompras_Services;

public class OfertaService : IOfertaService
{
    private IOfertaRepository _ofertaRepository;

    private ITipoProductoRepository _productoRepository;

    private IComercioRepository _comercioRepository;

    public OfertaService(IOfertaRepository ofertaRepository, ITipoProductoRepository productoRepository, IComercioRepository comercioRepository)
    {
        _ofertaRepository = ofertaRepository;
        _productoRepository = productoRepository;
        _comercioRepository = comercioRepository;
    }

    public List<OfertaDTO> ObtenerOfertasEconomicasPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida)
    {
        List<int> idProductos = ObtenerIdsTipoProductos(idBebida, idComida);

        List<OfertaDTO> ofertas = _ofertaRepository.OfertasPorLocalidad(idLocalidad, idProductos);

        return FiltrarOfertasEconomicasPorProducto(ofertas);
    }

    public List<OfertaDTO> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida)
    {
        List<OfertaDTO> listaCompraEconomica = new List<OfertaDTO>();

        List<int> idTiposProducto = ObtenerIdsTipoProductos(idBebida, idComida);

        //Recorrer cada producto que se necesita
        foreach (var idTipoProducto in idTiposProducto)
        {
            try
            {
                Decimal precioMinimo = _ofertaRepository.ObtenerPrecioMinimoDelProductoPorLocalidad(localidades, idTipoProducto);

                List<OfertaDTO> ofertas = _ofertaRepository.ObtenerOfertasPorPrecio(idTipoProducto, precioMinimo);

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

    public List<OfertaDTO> ObtenerOfertasEconomicasPorRadioGeografico(double latitud, double longitud, float distancia, int idComida, int idBebida)
    {
        List <int> idComercios = ObtenerIdsComerciosDentroDelRadio(latitud, longitud, distancia);

        List<int> idProductos = ObtenerIdsTipoProductos(idBebida, idComida);

        List<OfertaDTO> ofertas = _ofertaRepository.OfertasDentroDelRadio(idProductos, idComercios);

        return FiltrarOfertasEconomicasPorProducto(ofertas);
    }


    private List<int> ObtenerIdsTipoProductos(int idBebida, int idComida)
    {
        List<int> idProductos = new List<int>();
        List<int> idBebidas = _productoRepository.ObtenerIdTipoProductosBebida(idBebida);
        List<int> idComidas = _productoRepository.ObtenerIdTipoProductosComida(idComida);

        idProductos.AddRange(idBebidas);
        idProductos.AddRange(idComidas);

        return idProductos;
    }

    private List<int> ObtenerIdsComerciosDentroDelRadio(double latitud, double longitud, float distancia)
    {
        List<int> idComercios = new List<int>();

        List<Comercio> comercios = _comercioRepository.ObtenerComerciosPorRadio(latitud, longitud, distancia);

        foreach (var item in comercios)
        {
            idComercios.Add(item.Id);
        }
        return idComercios;
    }

    private List<OfertaDTO> FiltrarOfertasEconomicasPorProducto(List<OfertaDTO> ofertas)
    {
        List<OfertaDTO> ofertasMasEconomicas = new List<OfertaDTO>();

        int idMaximo = IdProductoMaximo(ofertas);

        do
        {
            OfertaDTO masEconomica = new OfertaDTO();
            masEconomica.Precio = 999999999999999999;

            for (int i = 0; i <= ofertas.Count() - 1; i++)
            {
                if (ofertas[i].IdTipoProducto == idMaximo && ofertas[i].Precio < masEconomica.Precio)
                {
                    masEconomica = ofertas[i];
                }
            }
            idMaximo--;

            if (masEconomica.IdTipoProducto != 0)
            {
                ofertasMasEconomicas.Add(masEconomica);
            }

        } while (idMaximo > 0);

        return ofertasMasEconomicas;
    }

    private int IdProductoMaximo(List<OfertaDTO> ofertas)
    {
        int idProdMáximo = 0;

        foreach (OfertaDTO item in ofertas)
        {
            if (item.IdTipoProducto > idProdMáximo)
            {
                idProdMáximo = item.IdTipoProducto;
            }
        }
        return idProdMáximo;
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
