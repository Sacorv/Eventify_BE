using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Repositories;
using Nest;
using System.Collections.Generic;

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


    public List<OfertaCantidadDTO> GenerarListaPersonalizada(FiltroDTO filtro)
    {
        List<OfertaCantidadDTO> listadoPersonalizado = new List<OfertaCantidadDTO>();

        List<OfertaDTO> ofertasDisponibles = BuscarOfertasDentroDelRadio(filtro);

        List<OfertaDTO> ofertasEconomicasSegunCantidadNecesaria = ExtaerMejoresOfertas(ofertasDisponibles, filtro.latitudUbicacion, filtro.longitudUbicacion, filtro.cantidadProductos);

        ofertasEconomicasSegunCantidadNecesaria.Sort((x, y) => x.TipoProducto.CompareTo(y.TipoProducto));

        ofertasEconomicasSegunCantidadNecesaria.ForEach(l => listadoPersonalizado.Add(new OfertaCantidadDTO() { cantidad = l.Peso != 0 ? Math.Ceiling(filtro.cantidadProductos[l.TipoProducto] / l.Peso) : Math.Ceiling(filtro.cantidadProductos[l.TipoProducto] / l.Unidades), ofertaDTO = l }));

        return listadoPersonalizado;
    }

    private List<OfertaDTO> BuscarOfertasDentroDelRadio(FiltroDTO filtro)
    {
        List<int> idComercios = BuscarComerciosDentroDelRadio(filtro.latitudUbicacion, filtro.longitudUbicacion, filtro.distancia);

        List<int> idProductos = BuscarProductosElegidos(filtro.bebidas, filtro.comidas);

        List<String> marcas = VerificarMarcasElegidas(filtro);

        return _ofertaRepository.OfertasDentroDelRadioV2(idProductos, idComercios, marcas);
    }

    private List<OfertaDTO> ExtaerMejoresOfertas(List<OfertaDTO> ofertasDisponibles, double latitudUbicacion, double longitudUbicacion, Dictionary<string, double> cantidadNecesaria)
    {
        List<OfertaDTO> ofertasMasEconomicas = new List<OfertaDTO>();

        ofertasDisponibles.Sort((x, y) => x.IdTipoProducto.CompareTo(y.IdTipoProducto));

        OfertaDTO ofertaMasEconomica = ofertasDisponibles[0];
        int idProducto = 1;

        for (int i=0; i<ofertasDisponibles.Count-1; i++)
        {
            if (ofertasDisponibles[i+1].IdTipoProducto != idProducto)
            {
                ofertasMasEconomicas.Add(ofertaMasEconomica);
                idProducto = ofertasDisponibles[i+1].IdTipoProducto;
                ofertaMasEconomica = ofertasDisponibles[i+1];
            }

            if (ofertasDisponibles[i+1].Peso != 0)
            {
                ofertaMasEconomica = CompararPreciosPorPesoNeto(ofertasDisponibles[i + 1], ofertaMasEconomica, latitudUbicacion, longitudUbicacion, cantidadNecesaria);
            }
            else
            {
                ofertaMasEconomica = CompararPreciosPorUnidad(ofertasDisponibles[i + 1], ofertaMasEconomica, latitudUbicacion, longitudUbicacion, cantidadNecesaria);
            }
        }

        ofertasMasEconomicas.Add(ofertaMasEconomica);

        return ofertasMasEconomicas;
    }

    private OfertaDTO CompararPreciosPorPesoNeto(OfertaDTO ofertaAComparar, OfertaDTO ofertaMasBarata, double latitudUbicacion, double longitudUbicacion, Dictionary<string, double> cantidadNecesaria)
    {
        OfertaDTO masEconomica = new OfertaDTO();

        double precioSegunCantidadNecesariaOfertaActual = (cantidadNecesaria[ofertaAComparar.TipoProducto] / ofertaAComparar.Peso) * ofertaAComparar.Precio;
        double precioSegunCantidadNecesariaOfertaBarata = (cantidadNecesaria[ofertaMasBarata.TipoProducto] / ofertaMasBarata.Peso) * ofertaMasBarata.Precio;


        if (precioSegunCantidadNecesariaOfertaActual < precioSegunCantidadNecesariaOfertaBarata)
        {
            masEconomica = ofertaAComparar;
        }
        else if (precioSegunCantidadNecesariaOfertaActual == precioSegunCantidadNecesariaOfertaBarata)
        {
            masEconomica = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertaAComparar, ofertaMasBarata);
        }
        else
        {
            masEconomica = ofertaMasBarata;
        }

        return masEconomica;
    }

    private OfertaDTO CompararPreciosPorUnidad(OfertaDTO ofertaAComparar, OfertaDTO ofertaMasBarata, double latitudUbicacion, double longitudUbicacion, Dictionary<string, double> cantidadNecesaria)
    {
        OfertaDTO masEconomica = new OfertaDTO();

        double precioSegunCantidadNecesariaOfertaActual = (cantidadNecesaria[ofertaAComparar.TipoProducto] / ofertaAComparar.Unidades) * ofertaAComparar.Precio;
        double precioSegunCantidadNecesariaOfertaBarata = (cantidadNecesaria[ofertaMasBarata.TipoProducto] / ofertaMasBarata.Unidades) * ofertaMasBarata.Precio;


        if (precioSegunCantidadNecesariaOfertaActual < precioSegunCantidadNecesariaOfertaBarata)
        {
            masEconomica = ofertaAComparar;
        }
        else if (precioSegunCantidadNecesariaOfertaActual == precioSegunCantidadNecesariaOfertaBarata)
        {
            masEconomica = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertaAComparar, ofertaMasBarata);
        }
        else
        {
            masEconomica = ofertaMasBarata;
        }

        return masEconomica;
    }

    private List<int> BuscarComerciosDentroDelRadio(double latitud, double longitud, float distancia)
    {
        return _comercioService.ObtenerComerciosPorRadio(latitud, longitud, distancia);
    }

    private List<int> BuscarProductosElegidos(List<int> idBebida, List<int> idComida)
    {
        List<int> idProductos = new List<int>();

        idProductos.AddRange(_tipoProductoService.ObtenerTiposDeBebida(idBebida));
        idProductos.AddRange(_tipoProductoService.ObtenerTiposDeComida(idComida));

        return idProductos;
    }

    private List<String> VerificarMarcasElegidas(FiltroDTO filtro)
    {
        List<String> marcasDeProductos = new List<string>();

        marcasDeProductos.AddRange(filtro.marcasBebida.Count!=0 ? filtro.marcasBebida : ObtenerMarcasDisponibles(filtro.bebidas));

        marcasDeProductos.AddRange(filtro.marcasComida.Count!=0 ? filtro.marcasComida : ObtenerMarcasDisponibles(filtro.comidas));

        return marcasDeProductos;
    }


    private List<string> ObtenerMarcasDisponibles(List<int> idProductos)
    {
        List<String> marcas = _ofertaRepository.ObtenerMarcasDisponibles(idProductos);

        return marcas;
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




    //public List<OfertaDTO> ObtenerOfertasEconomicasPorLocalidadPreferida(int idLocalidad, int idComida, int idBebida)
    //{
    //    List<int> idProductos = ObtenerIdsTipoProductos(idBebida, idComida);

    //    List<OfertaDTO> ofertas = _ofertaRepository.OfertasPorLocalidad(idLocalidad, idProductos);

    //    return FiltrarOfertasEconomicasPorProducto(ofertas);
    //}



    //TO DO: ver que no rompa nada y eliminar si se puede (SCA)
    //private List<OfertaDTO> FiltrarOfertasEconomicasPorProducto(List<OfertaDTO> ofertas, double latitudUbicacion=0, double longitudUbicacion=0)
    //{
    //    List<OfertaDTO> ofertasMasEconomicas = new List<OfertaDTO>();

    //    int idMaximo = IdProductoMaximo(ofertas);

    //    do
    //    {
    //        OfertaDTO ofertaMasEconomica = new OfertaDTO();
    //        ofertaMasEconomica.Precio = 999999999999999999;

    //        for (int i = 0; i <= ofertas.Count() - 1; i++)
    //        {
    //            if (ofertas[i].IdTipoProducto == idMaximo && ofertas[i].Precio <= ofertaMasEconomica.Precio)
    //            {
    //                if (ofertas[i].Precio < ofertaMasEconomica.Precio || latitudUbicacion==0)
    //                {
    //                    ofertaMasEconomica = ofertas[i];
    //                }
    //                else
    //                {
    //                    ofertaMasEconomica = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertas[i], ofertaMasEconomica);
    //                }
    //            }
    //        }
    //        idMaximo--;

    //        if (ofertaMasEconomica.IdTipoProducto != 0)
    //        {
    //            ofertasMasEconomicas.Add(ofertaMasEconomica);
    //        }

    //    } while (idMaximo > 0);

    //    return ofertasMasEconomicas;
    //}

    //TO DO: ver que no rompa nada y eliminar si se puede (SCA)
    //private int IdProductoMaximo(List<OfertaDTO> ofertas)
    //{
    //    int idProdMáximo = 0;

    //    foreach (OfertaDTO item in ofertas)
    //    {
    //        if (item.IdTipoProducto > idProdMáximo)
    //        {
    //            idProdMáximo = item.IdTipoProducto;
    //        }
    //    }
    //    return idProdMáximo;
    //}


}
