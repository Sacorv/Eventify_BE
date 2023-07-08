using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public class OfertaService : IOfertaService
{
    private IOfertaRepository _ofertaRepository;

    private IComercioService _comercioService;

    private ITipoProductoService _tipoProductoService;

    private IUbicacionService _ubicacionService;

    private DateTime fechaArgentina = DateTime.UtcNow.AddHours(-3).Date;

    public OfertaService(IOfertaRepository ofertaRepository, IComercioService comercioService, ITipoProductoService tipoProductoService, IUbicacionService ubicacionService)
    {
        _ofertaRepository = ofertaRepository;
        _comercioService = comercioService;
        _tipoProductoService = tipoProductoService;
        _ubicacionService = ubicacionService;
    }

    public List<Oferta> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida)
    {
        List<Oferta> listaCompraEconomica = new List<Oferta>();

        List<int> idTiposProducto = ObtenerIdsTipoProductos(idBebida, idComida);

        //Recorrer cada producto que se necesita
        foreach (var idTipoProducto in idTiposProducto)
        {
            try
            {
                Decimal precioMinimo = _ofertaRepository.ObtenerPrecioMinimoDelProductoPorLocalidad(localidades, idTipoProducto);

                List<Oferta> ofertas = _ofertaRepository.ObtenerOfertasPorPrecio(idTipoProducto, precioMinimo);

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


    public List<OfertasPorProducto> GenerarListaDeOfertas(Filtro filtro)
    {
        List<Oferta> ofertasDisponibles = BuscarOfertasDentroDelRadio(filtro);

        if (ofertasDisponibles.Count == 0)
        {
            return new List<OfertasPorProducto>();
        }

        ofertasDisponibles.Sort((x, y) => x.IdTipoProducto.CompareTo(y.IdTipoProducto));

        List<OfertaCantidad> ofertasCantidad = CalcularCantidadYSubtotalPorOferta(ofertasDisponibles, filtro.CantidadProductos);

        return GenerarListadoPorProductoYMarca(ofertasCantidad, filtro.LatitudUbicacion, filtro.LongitudUbicacion);
    }

    public List<OfertasPorComercioDTO> ListaRecorrerMenos(Filtro filtro)
    {
        List<OfertasPorComercioDTO> comercios = new List<OfertasPorComercioDTO>();
        List<Oferta> ofertas;
        List<OfertasPorComercioDTO> comerciosConLaMayorCantidadDeProductos = new List<OfertasPorComercioDTO>();
        int cantidadAComprar = filtro.CantidadProductos.Count;

        List<int> idComercios = _comercioService.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia);

        if (idComercios.Count == 0) return comercios;

        foreach (int idComercio in idComercios)
        {
            OfertasPorComercioDTO ofertaComercio = new OfertasPorComercioDTO();
            ofertas = _ofertaRepository.OfertasPorComercioFiltradasPorFecha(idComercio, fechaArgentina);
            ofertas = ProductosAComprarEnElComercio(filtro, ofertas);
            if (ofertas.Count > 0)
            {
                List<OfertaCantidad> ofertasConCantidades = CalcularCantidadYSubtotalPorOferta(ofertas, filtro.CantidadProductos);
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

    public bool VerficarSiLaOfertaExiste(int idComercio, int idProducto)
    {
        List<int> idProductosDelComercio = _ofertaRepository.ObtenerIdsProductosDelComercio(idComercio);
        return idProductosDelComercio.Contains(idProducto);
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

    private static List<Oferta> ProductosAComprarEnElComercio(Filtro filtro, List<Oferta> ofertas)
    {
        List<Oferta> ofertasEncontradas = new List<Oferta>();
        foreach (var productoAcomprar in filtro.CantidadProductos)
        {
            SeleccionarMarcasDelProducto(ofertas, ofertasEncontradas, productoAcomprar);
        }
        return ofertasEncontradas;
    }

    private static void SeleccionarMarcasDelProducto(List<Oferta> ofertas, List<Oferta> ofertasEncontradas, KeyValuePair<string, double> productoAcomprar)
    {
        List<Oferta>? encontrados = ofertas.Where(o => o.TipoProducto == productoAcomprar.Key).ToList();
        if (encontrados.Count > 0) ofertasEncontradas.Add(encontrados.First());
    }

    private List<OfertasPorProducto> GenerarListadoPorProductoYMarca(List<OfertaCantidad> ofertasCantidad, double latitudUbicacion, double longitudUbicacion)
    {
        List<OfertasPorProducto> ofertasDisponiblesAgrupadasPorProducto = new List<OfertasPorProducto>();

        List<OfertaCantidad> ofertasProducto = new List<OfertaCantidad>();

        ofertasCantidad.Sort((x, y) => x.Oferta.IdTipoProducto.CompareTo(y.Oferta.IdTipoProducto));


        int idTipoProducto = ofertasCantidad[0].Oferta.IdTipoProducto;
        OfertaCantidad actual = null;

        foreach (OfertaCantidad oferta in ofertasCantidad)
        {
            if (oferta.Oferta.IdTipoProducto == idTipoProducto)
            {
                actual = oferta;
                ofertasProducto.Add(oferta);
            }
            else
            {
                ofertasDisponiblesAgrupadasPorProducto.Add(new OfertasPorProducto() { NombreProducto = actual.Oferta.TipoProducto, Ofertas = MenorPrecioPorProductoYMarca(ofertasProducto, latitudUbicacion, longitudUbicacion) });
                ofertasProducto = new List<OfertaCantidad>();
                idTipoProducto = oferta.Oferta.IdTipoProducto;
                actual = oferta;
                ofertasProducto.Add(oferta);
            }
        }
        ofertasDisponiblesAgrupadasPorProducto.Add(new OfertasPorProducto() { NombreProducto = actual.Oferta.TipoProducto, Ofertas = MenorPrecioPorProductoYMarca(ofertasProducto, latitudUbicacion, longitudUbicacion) });

        return ofertasDisponiblesAgrupadasPorProducto;
    }

    private List<OfertaCantidad> MenorPrecioPorProductoYMarca(List<OfertaCantidad> ofertas, double latitudUbicacion, double longitudUbicacion)
    {
        Oferta oferta = new Oferta();
        List<OfertaCantidad> ofertasFiltradas = new List<OfertaCantidad>();
        ofertas.Sort((x, y) => x.Oferta.Marca.CompareTo(y.Oferta.Marca));

        String marcaActual = ofertas[0].Oferta.Marca;
        OfertaCantidad ofertaMasEconomica = ofertas[0];

        foreach (OfertaCantidad ofertaActual in ofertas)
        {
            if (ofertaActual.Oferta.Marca.Equals(marcaActual))
            {
                if(ofertaActual.Subtotal < ofertaMasEconomica.Subtotal)
                {
                    ofertaMasEconomica = ofertaActual;
                }
                else if(ofertaActual.Subtotal==ofertaMasEconomica.Subtotal)
                {
                    oferta = _comercioService.CompararDistanciaEntreComercios(latitudUbicacion, longitudUbicacion, ofertaActual.Oferta, ofertaMasEconomica.Oferta);
                    ofertaMasEconomica = ofertaActual.Oferta.IdPublicacion == oferta.IdPublicacion ? ofertaActual : ofertaMasEconomica;
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

    public List<Oferta> BuscarOfertasDentroDelRadio(Filtro filtro)
    {
        List<int> idComercios = BuscarComerciosDentroDelRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia);
        if(idComercios.Count == 0)
        {
            return new List<Oferta>();
        }

        List<int> idProductos = BuscarProductosElegidos(filtro.Bebidas, filtro.Comidas);

        if (idProductos.Count==0)
        {
            return new List<Oferta>();
        }

        List<String> marcas = VerificarMarcasElegidas(filtro);

        List<Oferta> ofertasEncontradas = _ofertaRepository.OfertasDentroDelRadioV2(idProductos, idComercios, marcas);

        return FiltrarOfertasVigentes(ofertasEncontradas);
    }

    private List<Oferta> FiltrarOfertasVigentes(List<Oferta> ofertasEncontradas)
    {
        List<Oferta> listadoOfertas = new List<Oferta>();

        foreach (Oferta oferta in ofertasEncontradas)
        {
            if (DateTime.Compare(Convert.ToDateTime(oferta.FechaVencimiento).Date, fechaArgentina) > 0)
            {
                listadoOfertas.Add(oferta);
            }
        }
        return listadoOfertas;
    }


    private List<OfertaCantidad> CalcularCantidadYSubtotalPorOferta(List<Oferta> ofertasEconomicas, Dictionary<string, double> cantidadProductos)
    {
        List<OfertaCantidad> listadoPersonalizado = new List<OfertaCantidad>();

        ofertasEconomicas.Sort((x, y) => x.TipoProducto.CompareTo(y.TipoProducto));

        ofertasEconomicas.ForEach(oferta => listadoPersonalizado.Add(new OfertaCantidad() 
                                                                                { Cantidad = oferta.Peso != 0 ? Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Peso) : Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Unidades), 
                                                                                  Oferta = oferta, 
                                                                                  Subtotal = oferta.Peso != 0 ? Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Peso) * oferta.Precio : Math.Ceiling(cantidadProductos[oferta.TipoProducto] / oferta.Unidades) * oferta.Precio }));

        return listadoPersonalizado;
    }

    private Oferta CompararPreciosPorPesoNeto(Oferta ofertaAComparar, Oferta ofertaMasBarata, double latitudUbicacion, double longitudUbicacion, Dictionary<string, double> cantidadNecesaria)
    {
        Oferta masEconomica = new Oferta();

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

    private Oferta CompararPreciosPorUnidad(Oferta ofertaAComparar, Oferta ofertaMasBarata, double latitudUbicacion, double longitudUbicacion, Dictionary<string, double> cantidadNecesaria)
    {
        Oferta masEconomica = new Oferta();

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
        List<int> idBebidas = _tipoProductoService.ObtenerTiposDeBebida(idBebida);
        List<int> idComidas = _tipoProductoService.ObtenerTiposDeComida(idComida);

        if(idBebidas.Count==0 || idComidas.Count==0)
        {
            return idProductos;
        }
        else
        {
            idProductos.AddRange(idBebidas);
            idProductos.AddRange(idComidas);
        }
        return idProductos;
    }

    private List<String> VerificarMarcasElegidas(Filtro filtro)
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

    private static Oferta SeleccionarOferta(List<int> localidades, List<Oferta> ofertas)
    {
        Oferta recomendacion = new Oferta();

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
