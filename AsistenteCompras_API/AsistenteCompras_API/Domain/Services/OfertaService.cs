using AsistenteCompras_API.DTOs;
using System.Linq;

namespace AsistenteCompras_API.Domain.Services;

public class OfertaService : IOfertaService
{
    private IOfertaRepository _ofertaRepository;

    private IComercioService _comercioService;

    private ITipoProductoService _tipoProductoService;

    private IUbicacionService _ubicacionService;

    public OfertaService(IOfertaRepository ofertaRepository, IComercioService comercioService, ITipoProductoService tipoProductoService, IUbicacionService ubicacionService)
    {
        _ofertaRepository = ofertaRepository;
        _comercioService = comercioService;
        _tipoProductoService = tipoProductoService;
        _ubicacionService = ubicacionService;
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

        if (ofertasDisponibles.Count == 0)
        {
            return new List<OfertasPorProductoDTO>();
        }

        ofertasDisponibles.Sort((x, y) => x.IdTipoProducto.CompareTo(y.IdTipoProducto));

        List<OfertaCantidadDTO> ofertasCantidad = CalcularCantidadYSubtotalPorOferta(ofertasDisponibles, filtro.CantidadProductos);

        return GenerarListadoPorProductoYMarca(ofertasCantidad, filtro.LatitudUbicacion, filtro.LongitudUbicacion);
    }

    public List<OfertasPorComercioDTO> ListaRecorrerMenos(FiltroDTO filtro)
    {
        List<OfertasPorComercioDTO> comercios = new List<OfertasPorComercioDTO>();
        List<OfertaDTO> ofertas;
        List<OfertasPorComercioDTO> comerciosConLaMayorCantidadDeProductos = new List<OfertasPorComercioDTO>();
        int cantidadAComprar = filtro.CantidadProductos.Count;

        List<int> idComercios = _comercioService.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia);

        if (idComercios.Count == 0) return comercios;

        foreach (int idComercio in idComercios)
        {
            OfertasPorComercioDTO ofertaComercio = new OfertasPorComercioDTO();
            ofertas = _ofertaRepository.OfertasPorComercio(idComercio);
            ofertas = ProductosAComprarEnElComercio(filtro, ofertas);
            if (ofertas.Count > 0)
            {
                List<OfertaCantidadDTO> ofertasConCantidades = CalcularCantidadYSubtotalPorOferta(ofertas, filtro.CantidadProductos);
                ofertaComercio.Ofertas = ofertasConCantidades;
                ofertaComercio.NombreComercio = ofertasConCantidades.First().Oferta.NombreComercio;
                ofertaComercio.ImagenComercio = _comercioService.ObtenerImagenDelComercio(idComercio);
                comercios.Add(ofertaComercio);
            }
        }

        BuscarComerciosConLaMayorCantidadDeProductos(comercios, comerciosConLaMayorCantidadDeProductos);

        if (TieneTodosLosProductos(comerciosConLaMayorCantidadDeProductos, cantidadAComprar))
        {
            AgregarDistanciaAComercios(comerciosConLaMayorCantidadDeProductos, filtro.LatitudUbicacion, filtro.LongitudUbicacion);
            comerciosConLaMayorCantidadDeProductos.Sort((x, y) => x.Distancia.CompareTo(y.Distancia));
        }
        else { comerciosConLaMayorCantidadDeProductos.Clear(); }
        
        return comerciosConLaMayorCantidadDeProductos;
    }

    private void RecomendarComercio(List<OfertasPorComercioDTO>aRecomendar,List<OfertasPorComercioDTO> actual, double latitudOrigen, double longitudOrigen)
    {
        double longitudDestino = aRecomendar.Select(a => a.Ofertas.First().Oferta.Longitud).First();
        double latitudDestino = aRecomendar.Select(a => a.Ofertas.First().Oferta.Latitud).First();
        actual.Clear();
        actual.Add(aRecomendar.First());
        double menorDistanciaActual = _ubicacionService.CalcularDistanciaPorHaversine(latitudOrigen,longitudOrigen,latitudDestino,longitudDestino);
        double menorDistanciaEncontrada;
        if(aRecomendar.Count > 1)
        {
            for(int i = 1; i > aRecomendar.Count; i++)
            {
                longitudDestino = aRecomendar[i].Ofertas.First().Oferta.Longitud;
                latitudDestino = aRecomendar[i].Ofertas.First().Oferta.Longitud;
                menorDistanciaEncontrada = _ubicacionService.CalcularDistanciaPorHaversine(latitudOrigen, longitudOrigen, latitudDestino, longitudDestino);
                if(menorDistanciaEncontrada < menorDistanciaActual)
                {
                    menorDistanciaActual = menorDistanciaEncontrada;
                    actual.Clear();
                    actual.Add(aRecomendar[i]);
                }
            }
        }

    }

    private void AgregarDistanciaAComercios(List<OfertasPorComercioDTO> aRecomendar, double latitudOrigen, double longitudOrigen)
    {
        foreach(OfertasPorComercioDTO comercio in aRecomendar)
        {
            double longitudDestino = comercio.Ofertas.First().Oferta.Longitud;
            double latitudDestino = comercio.Ofertas.First().Oferta.Latitud;
            comercio.Distancia = _ubicacionService.CalcularDistanciaPorHaversine(latitudOrigen, longitudOrigen, latitudDestino, longitudDestino);
        }
    }

    private static bool TieneTodosLosProductos(List<OfertasPorComercioDTO> comerciosConLaMayorCantidadDeProductos, int cantidadAComprar)
    {
        if(comerciosConLaMayorCantidadDeProductos.Count > 0)
        {
            int cantidadDeProductosEnElComercio = comerciosConLaMayorCantidadDeProductos.First().Ofertas.DistinctBy(o => o.Oferta.TipoProducto).Count();
            if (cantidadDeProductosEnElComercio == cantidadAComprar)
                return true;
        }

        return false;
    }

    private static List<OfertasPorComercioDTO> BuscarComerciosConLaMayorCantidadDeProductos(List<OfertasPorComercioDTO> comercios, List<OfertasPorComercioDTO> comerciosConLaMayorCantidadDeProductos)
    {
        int cantidadProductosDelComercioActual = 0;
        int cantidadProductosDelComercioMaxima = 0;

        foreach (var comercio in comercios)
        {
            cantidadProductosDelComercioActual = comercio.Ofertas.DistinctBy(c => c.Oferta.TipoProducto).Count();
            if(cantidadProductosDelComercioActual >= cantidadProductosDelComercioMaxima)
            {
                if(cantidadProductosDelComercioActual > cantidadProductosDelComercioMaxima) comerciosConLaMayorCantidadDeProductos.Clear();
                comerciosConLaMayorCantidadDeProductos.Add(comercio);
                cantidadProductosDelComercioMaxima = cantidadProductosDelComercioActual;
            }
        }

        return comerciosConLaMayorCantidadDeProductos;
    }

    private static List<OfertaDTO> ProductosAComprarEnElComercio(FiltroDTO filtro, List<OfertaDTO> ofertas)
    {
        List<OfertaDTO> ofertasEncontradas = new List<OfertaDTO>();
        foreach (var productoAcomprar in filtro.CantidadProductos)
        {
            SeleccionarMarcasDelProducto(ofertas, ofertasEncontradas, productoAcomprar);
        }
        return ofertasEncontradas;
    }

    private static void SeleccionarMarcasDelProducto(List<OfertaDTO> ofertas, List<OfertaDTO> ofertasEncontradas, KeyValuePair<string, double> productoAcomprar)
    {
        List<OfertaDTO>? encontrados = ofertas.Where(o => o.TipoProducto == productoAcomprar.Key).ToList();
        if (encontrados.Count > 0) ofertasEncontradas.Add(encontrados.First());
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

        ofertasFiltradas.Sort((x, y) => x.Subtotal.CompareTo(y.Subtotal));

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
}
