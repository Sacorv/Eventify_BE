﻿using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

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
        List<OfertaDTO> ofertasDisponibles = BuscarOfertasDentroDelRadio(filtro);

        List<OfertaDTO> ofertasEconomicasSegunCantidadNecesaria = ExtaerMejoresOfertas(ofertasDisponibles, filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.CantidadProductos);

        return CalcularCantidadYSubtotalPorOferta(ofertasEconomicasSegunCantidadNecesaria, filtro.CantidadProductos);
    }

    public List<OfertasPorProductoDTO> GenerarListaDeOfertas(FiltroDTO filtro)
    {
        List<OfertaDTO> ofertasDisponibles = BuscarOfertasDentroDelRadio(filtro);

        ofertasDisponibles.Sort((x, y) => x.IdTipoProducto.CompareTo(y.IdTipoProducto));

        List<OfertaCantidadDTO> ofertasCantidad = CalcularCantidadYSubtotalPorOferta(ofertasDisponibles, filtro.CantidadProductos);

        return GenerarListadoPorProductoYMarca(ofertasCantidad, filtro.LatitudUbicacion, filtro.LongitudUbicacion);
    }
    private List<OfertasPorProductoDTO> GenerarListadoPorProductoYMarca(List<OfertaCantidadDTO> ofertasCantidad, double latitudUbicacion, double longitudUbicacion)
    {
        List<OfertasPorProductoDTO> ofertasDisponiblesAgrupadasPorProducto = new List<OfertasPorProductoDTO>();

        List<OfertaCantidadDTO> ofertasProducto = new List<OfertaCantidadDTO>();

        ofertasCantidad.Sort((x, y) => x.Oferta.IdTipoProducto.CompareTo(y.Oferta.IdTipoProducto));


        int idTipoProducto = ofertasCantidad[0].Oferta.IdTipoProducto;
        OfertaCantidadDTO actual = null;

        foreach (OfertaCantidadDTO oferta in ofertasCantidad)
        {
            if (oferta.Oferta.IdTipoProducto == idTipoProducto)
            {
                actual = oferta;
                ofertasProducto.Add(oferta);
            }
            else
            {
                ofertasDisponiblesAgrupadasPorProducto.Add(new OfertasPorProductoDTO() { NombreProducto = actual.Oferta.TipoProducto, Ofertas = MenorPrecioPorProductoYMarca(ofertasProducto, latitudUbicacion, longitudUbicacion) });
                ofertasProducto = new List<OfertaCantidadDTO>();
                idTipoProducto = oferta.Oferta.IdTipoProducto;
                actual = oferta;
                ofertasProducto.Add(oferta);
            }
        }
        ofertasDisponiblesAgrupadasPorProducto.Add(new OfertasPorProductoDTO() { NombreProducto = actual.Oferta.TipoProducto, Ofertas = MenorPrecioPorProductoYMarca(ofertasProducto, latitudUbicacion, longitudUbicacion) });

        return ofertasDisponiblesAgrupadasPorProducto;
    }

    private List<OfertaCantidadDTO> MenorPrecioPorProductoYMarca(List<OfertaCantidadDTO> ofertas, double latitudUbicacion, double longitudUbicacion)
    {
        OfertaDTO ofertaDTO = new OfertaDTO();
        List<OfertaCantidadDTO> ofertasFiltradas = new List<OfertaCantidadDTO>();
        ofertas.Sort((x, y) => x.Oferta.Marca.CompareTo(y.Oferta.Marca));

        String marcaActual = ofertas[0].Oferta.Marca;
        OfertaCantidadDTO ofertaMasEconomica = ofertas[0];

        foreach (OfertaCantidadDTO ofertaActual in ofertas)
        {
            if (ofertaActual.Oferta.Marca.Equals(marcaActual))
            {
                if(ofertaActual.Subtotal < ofertaMasEconomica.Subtotal)
                {
                    ofertaMasEconomica = ofertaActual;
                }
                else if(ofertaActual.Subtotal==ofertaMasEconomica.Subtotal)
                {
                    ofertaDTO = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertaActual.Oferta, ofertaMasEconomica.Oferta);
                    ofertaMasEconomica = ofertaActual.Oferta.IdPublicacion == ofertaDTO.IdPublicacion ? ofertaActual : ofertaMasEconomica;
                }
            }
            else
            {
                ofertasFiltradas.Add(ofertaMasEconomica);
                marcaActual = ofertaActual.Oferta.Marca;
                ofertaMasEconomica = ofertaActual;
            }
        }

        ofertasFiltradas.Add(ofertaMasEconomica);

        return ofertasFiltradas;
    }

    private List<OfertaDTO> BuscarOfertasDentroDelRadio(FiltroDTO filtro)
    {
        List<int> idComercios = BuscarComerciosDentroDelRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia);

        List<int> idProductos = BuscarProductosElegidos(filtro.Bebidas, filtro.Comidas);

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

    private List<OfertaCantidadDTO> CalcularCantidadYSubtotalPorOferta(List<OfertaDTO> ofertasEconomicas, Dictionary<string, double> cantidadProductos)
    {
        List<OfertaCantidadDTO> listadoPersonalizado = new List<OfertaCantidadDTO>();

        ofertasEconomicas.Sort((x, y) => x.TipoProducto.CompareTo(y.TipoProducto));

        ofertasEconomicas.ForEach(oferta => listadoPersonalizado.Add(new OfertaCantidadDTO() 
                                                                                { Cantidad = oferta.Peso != 0 ? Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Peso) : Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Unidades), 
                                                                                  Oferta = oferta, 
                                                                                  Subtotal = oferta.Peso != 0 ? Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Peso) * oferta.Precio : Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Unidades) * oferta.Precio }));

        return listadoPersonalizado;
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

        marcasDeProductos.AddRange(filtro.MarcasBebida==null || filtro.MarcasBebida.Count==0 ? ObtenerMarcasBebidasDisponibles(filtro.Bebidas) : filtro.MarcasBebida);

        marcasDeProductos.AddRange(filtro.MarcasComida == null || filtro.MarcasComida.Count==0 ? ObtenerMarcasComidasDisponibles(filtro.Comidas) : filtro.MarcasComida);

        return marcasDeProductos;
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

    private List<string> ObtenerMarcasBebidasDisponibles(List<int> idProductos)
    {
        List<String> marcas = _ofertaRepository.ObtenerMarcasBebidasDisponibles(idProductos);

        return marcas;
    }


    private List<string> ObtenerMarcasComidasDisponibles(List<int> idProductos)
    {
        List<String> marcas = _ofertaRepository.ObtenerMarcasComidasDisponibles(idProductos);

        return marcas;
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