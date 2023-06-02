using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Repositories;
using Nest;

namespace AsistenteCompras_Services;

public class OfertaService : IOfertaService
{
    private IOfertaRepository _ofertaRepository;

    private IComercioService _comercioService;

    private ITipoProductoService _tipoProductoService;

    public OfertaService(IOfertaRepository ofertaRepository, IComercioService comercioService, ITipoProductoService tipoProductoService)
    {
        _ofertaRepository = ofertaRepository;
        _comercioService = comercioService;
        _tipoProductoService = tipoProductoService;
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

    public List<OfertaDTO> ObtenerOfertasEconomicasPorRadioGeografico(double latitudUbicacion, double longitudUbicacion, float distanciaRadio, int idComida, int idBebida)
    {
        List <int> idComercios = ObtenerIdsComerciosDentroDelRadio(latitudUbicacion, longitudUbicacion, distanciaRadio);

        List<int> idProductos = ObtenerIdsTipoProductos(idBebida, idComida);

        List<OfertaDTO> ofertas = _ofertaRepository.OfertasDentroDelRadio(idProductos, idComercios);

        return FiltrarOfertasEconomicasPorProducto(ofertas, latitudUbicacion, longitudUbicacion);
    }

    public List<OfertaDTO> GenerarListaPersonalizada(FiltroDTO filtro)
    {
        List<int> idComercios = ObtenerIdsComerciosDentroDelRadio(filtro.latitudUbicacion, filtro.longitudUbicacion, filtro.distancia);

        List<int> idProductos = ObtenerIdsTipoProductosV2(filtro.bebidas, filtro.comidas);

        List<String> marcas = VerificarMarcasABuscar(filtro.marcasBebida, filtro.marcasComida, filtro.bebidas, filtro.comidas);

        List<OfertaDTO> ofertas = _ofertaRepository.OfertasDentroDelRadioV2(idProductos, idComercios, marcas);

        return FiltrarOfertasEconomicasPorProducto(ofertas, filtro.latitudUbicacion, filtro.longitudUbicacion);
    }

   

    private List<String> VerificarMarcasABuscar(List<String> marcasBebida, List<String> marcasComida, List<int> idBebidas, List<int> idComidas)
    {
        List<String> marcas = new List<string>();

        if (marcasBebida.Count != 0)
        {
            marcas.AddRange(marcasBebida);
        }
        else
        {
            marcas.AddRange(ObtenerMarcasDisponibles(ObtenerIdsTipoProductosBebidaV2(idBebidas)));
        }
        if (marcasComida.Count != 0)
        {
            marcas.AddRange(marcasComida);
        }
        else
        {
            marcas.AddRange(ObtenerMarcasDisponibles(ObtenerIdsTipoProductosComidaV2(idComidas)));
        }

        return marcas;
    }

    private List<string> ObtenerMarcasDisponibles(List<int> idProductos)
    {
        List<String> marcas = _ofertaRepository.ObtenerMarcasDisponibles(idProductos);

        return marcas;
    }

    private List<int> ObtenerIdsTipoProductosV2(List<int> idBebida, List<int> idComida)
    {
        List<int> idProductos = new List<int>();
        List<int> idBebidas = _tipoProductoService.ObtenerIdTipoProductosBebidaV2(idBebida);
        List<int> idComidas = _tipoProductoService.ObtenerIdTipoProductosComidaV2(idComida);

        idProductos.AddRange(idBebidas);
        idProductos.AddRange(idComidas);

        return idProductos;
    }

    private List<int> ObtenerIdsTipoProductosComidaV2(List<int> idComida)
    {
        List<int> idProductos = new List<int>();
  
        List<int> idComidas = _tipoProductoService.ObtenerIdTipoProductosComidaV2(idComida);

        idProductos.AddRange(idComidas);

        return idProductos;
    }

    private List<int> ObtenerIdsTipoProductosBebidaV2(List<int> idBebida)
    {
        List<int> idProductos = new List<int>();
        List<int> idBebidas = _tipoProductoService.ObtenerIdTipoProductosBebidaV2(idBebida);

        idProductos.AddRange(idBebidas);

        return idProductos;
    }


    private List<int> ObtenerIdsTipoProductos(int idBebida, int idComida)
    {
        List<int> idProductos = new List<int>();
        List<int> idBebidas = _tipoProductoService.ObtenerIdTipoProductosBebida(idBebida);
        List<int> idComidas = _tipoProductoService.ObtenerIdTipoProductosComida(idComida);

        idProductos.AddRange(idBebidas);
        idProductos.AddRange(idComidas);

        return idProductos;
    }

    private List<int> ObtenerIdsComerciosDentroDelRadio(double latitud, double longitud, float distancia)
    {
        List<int> idComercios = new List<int>();

        List<Comercio> comercios = _comercioService.ObtenerComerciosPorRadio(latitud, longitud, distancia);

        foreach (var item in comercios)
        {
            idComercios.Add(item.Id);
        }
        return idComercios;
    }

    private List<OfertaDTO> FiltrarOfertasEconomicasPorProducto(List<OfertaDTO> ofertas, double latitudUbicacion=0, double longitudUbicacion=0)
    {
        List<OfertaDTO> ofertasMasEconomicas = new List<OfertaDTO>();

        int idMaximo = IdProductoMaximo(ofertas);

        do
        {
            OfertaDTO ofertaMasEconomica = new OfertaDTO();
            ofertaMasEconomica.Precio = 999999999999999999;

            for (int i = 0; i <= ofertas.Count() - 1; i++)
            {
                if (ofertas[i].IdTipoProducto == idMaximo && ofertas[i].Precio <= ofertaMasEconomica.Precio)
                {
                    if (ofertas[i].Precio < ofertaMasEconomica.Precio || latitudUbicacion==0)
                    {
                        ofertaMasEconomica = ofertas[i];
                    }
                    else
                    {
                        ofertaMasEconomica = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertas[i], ofertaMasEconomica);
                    }
                }
            }
            idMaximo--;

            if (ofertaMasEconomica.IdTipoProducto != 0)
            {
                ofertasMasEconomicas.Add(ofertaMasEconomica);
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
