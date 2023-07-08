using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using Moq;

namespace AsistenteCompras_Tests.Service;

public class OfertaServiceTest
{
    private static Mock<IOfertaRepository> ofertaRepo = new Mock<IOfertaRepository>();

    private static Mock<IComercioService> comercioServicio = new Mock<IComercioService>();

    private static Mock<ITipoProductoService> tipoProductoServicio = new Mock<ITipoProductoService>();

    private static Mock<IUbicacionService> ubicacionServicio = new Mock<IUbicacionService>();

    private DateTime fechaArgentina = DateTime.UtcNow.AddHours(-3).Date;

    private DateTime fechaPrueba = DateTime.UtcNow.AddDays(+30).Date;

    private OfertaService ofertaServicio = new OfertaService(ofertaRepo.Object, comercioServicio.Object, tipoProductoServicio.Object, ubicacionServicio.Object);

    private const double LATITUDUNO = -34.653976;
    private const double LONGITUDUNO = -58.618469;

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaVaciaCuandoNoHayComerciosEnElRadio()
    {
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 } }
        };

        List<int> comerciosEncontrados = new List<int>();

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia))
                        .Returns(comerciosEncontrados);

        var resultado = ofertaServicio.ListaRecorrerMenos(filtro);

        Assert.Empty(resultado);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaVaciaCuandoNoHayOfertasParaLosProductosQueQuieroComprar()
    {
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 } }
        };

        List<int> comerciosEncontados = new List<int>() { 1 };

        List<Oferta> ofertasDelComercio = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 15,
                IdTipoProducto = 8,
                TipoProducto = "Snacks",
                NombreProducto = "Papa Fritas Lays",
                Marca = "Lays",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 350,
                Unidades = 1,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = 1,
                Longitud = 1,
                FechaVencimiento = fechaArgentina.ToString("dd-MM-yy")
            }
        };

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia))
                        .Returns(comerciosEncontados);

        List<Oferta> oferta = new List<Oferta>();

        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(1, fechaArgentina.Date)).Returns(ofertasDelComercio);


        var resultado = ofertaServicio.ListaRecorrerMenos(filtro);
        comercioServicio.Verify(c => c.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia));
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(1, fechaArgentina));
        Assert.Empty(resultado);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaDeComerciosConLasOfertasDeTodosMisProductosAComprar()
    {
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, { "gaseosa", 1 } }
        };

        List<int> comerciosEncontados = new List<int>() { 1, 2 };

        List<Oferta> ofertasDelComercioChino = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 15,
                IdTipoProducto = 8,
                TipoProducto = "Snacks",
                NombreProducto = "Papa Fritas Lays",
                Marca = "Lays",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 350,
                Unidades = 1,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68479,
                Longitud = -58.50380
            }
        };

        List<Oferta> ofertasDelComercioAlmacen = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 4,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Coca-cola 1.5lt",
                Marca = "Coca-cola",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            }


        };

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia))
                        .Returns(comerciosEncontados);

        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina)).Returns(ofertasDelComercioChino);
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1], fechaArgentina)).Returns(ofertasDelComercioAlmacen);

        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[0])).Returns("ImagenChino");
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[1])).Returns("ImagenAlmacen");

        var resultado = ofertaServicio.ListaRecorrerMenos(filtro);

        Assert.Equal("Almacen", resultado.First().NombreComercio);
    }

    /*[Fact]
    public void QueObtengaUnaListaVacíaDeOfertasMejoresPreciosEnRadioSinComercios()
    {
        Filtro filtros = DadoUnSetDeDatosConRadioDeDistanciaReducido();
        List<OfertasPorProducto> resultado = CuandoBuscoOfertasDentroDelRadioReducido(filtros);
        EntoncesObtengoUnaListaVacía(resultado);
    }

    private Filtro DadoUnSetDeDatosConRadioDeDistanciaReducido()
    {
        return new Filtro
        {
            LatitudUbicacion = -34.691804207148479,
            LongitudUbicacion = -58.572354315470506,
            Distancia = 1,
            Comidas = new List<int>() { 1 },
            Bebidas = new List<int>() { 2 },
            CantidadProductos = new Dictionary<string, double>() { { "Harina", 2000 }, { "Queso", 2000 }, { "Salsa de tomate", 1000 }, { "Gaseosa", 5000 }, { "Jugo", 4000 } }
        };
    }

    private List<OfertasPorProducto> CuandoBuscoOfertasDentroDelRadioReducido(Filtro filtros)
    {
        List<int> comerciosEncontrados = new List<int>();

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtros.LatitudUbicacion, filtros.LongitudUbicacion, filtros.Distancia))
                        .Returns(comerciosEncontrados);

        return ofertaServicio.GenerarListaDeOfertas(filtros);
    }

    private void EntoncesObtengoUnaListaVacía(List<OfertasPorProducto> ofertas)
    {
        Assert.True(ofertas.Count == 0);
    }

    [Fact]
    public void QueNoPuedaBuscarOfertasSiEnvioTiposComidaYBebidaInexistentes()
    {
        Filtro filtros = DadoUnSetDeDatosConTipoComidaYBebidaInexistente();
        List<OfertasPorProducto> listaVacía = CuandoBuscoOfertas(filtros);
        EntoncesObtengoUna(listaVacía);
    }


    private Filtro DadoUnSetDeDatosConTipoComidaYBebidaInexistente()
    {
        return new Filtro
        {
            LatitudUbicacion = -34.691804207148479,
            LongitudUbicacion = -58.572354315470506,
            Distancia = 10,
            Comidas = new List<int>() { 20 },
            Bebidas = new List<int>() { 25 },
            CantidadProductos = new Dictionary<string, double>() { { "Harina", 2000 }, { "Queso", 2000 }, { "Salsa de tomate", 1000 }, { "Gaseosa", 5000 }, { "Jugo", 4000 } }
        };
    }

    private List<OfertasPorProducto> CuandoBuscoOfertas(Filtro filtros)
    {
        List<int> idComercios = new List<int>() { 10, 12 };

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtros.LatitudUbicacion, filtros.LongitudUbicacion, filtros.Distancia))
                        .Returns(idComercios);


        List<int> idProductos = new List<int>();
        List<int> idBebidas = new List<int>();
        List<int> idComidas = new List<int>();
        tipoProductoServicio.Setup(b => b.ObtenerTiposDeBebida(filtros.Bebidas)).Returns(idBebidas);
        tipoProductoServicio.Setup(b => b.ObtenerTiposDeComida(filtros.Comidas)).Returns(idComidas);

        idProductos.AddRange(idBebidas);
        idProductos.AddRange(idComidas);

        List<Oferta> ofertasEncontradas = new List<Oferta>();

        return ofertaServicio.GenerarListaDeOfertas(filtros);
    }

    private void EntoncesObtengoUna(List<OfertasPorProducto> listaVacía)
    {
        Assert.Empty(listaVacía);
    }

    [Fact]
    public void QuePuedaObtenerOfertasMejoresPreciosDentroDeUnRadio()
    {
        Filtro filtros = DadoUnSetDeDatos();
        List<OfertasPorProducto> mejoresOfertas = CuandoBuscoOfertasDentroDelRadio(filtros);
        EntoncesObtengoUnaLista(mejoresOfertas);
    }

    private Filtro DadoUnSetDeDatos()
    {
        return new Filtro
        {
            LatitudUbicacion = -34.691804207148479,
            LongitudUbicacion = -58.572354315470506,
            Distancia = 6,
            Comidas = new List<int>() { 1 },
            Bebidas = new List<int>() { 2 },
            CantidadProductos = new Dictionary<string, double>() { { "Harina", 2000 }, { "Queso", 2000 }, { "Salsa de tomate", 1000 }, { "Gaseosa", 5000 }, { "Jugo", 4000 } }
        };
    }

    private List<OfertasPorProducto> CuandoBuscoOfertasDentroDelRadio(Filtro filtros)
    {
        List<int> idComercios = ComerciosDentroDelRadio(filtros);

        List<int> idProductos = IdTipoProductos(filtros);

        List<string> marcas = MarcasElegidas(filtros.Bebidas, filtros.Comidas);

        List<Oferta> ofertasEncontradas = OfertasEncontradas(idComercios, idProductos, marcas, filtros.CantidadProductos);

        Oferta oferta = new Oferta();
        oferta = ofertasEncontradas[1];
        comercioServicio.Setup(c => c.CompararDistanciaEntreComercios(filtros.LatitudUbicacion, filtros.LongitudUbicacion, ofertasEncontradas[0], ofertasEncontradas[1])).Returns(oferta);

        List<OfertasPorProducto> ofertas = ofertaServicio.GenerarListaDeOfertas(filtros);

        return ofertas;
    }

    private void EntoncesObtengoUnaLista(List<OfertasPorProducto> ofertas)
    {
        Assert.True(ofertas.Count != 0);
    }

    private List<Oferta> SetDeOfertasEjemplo(int cantidadOfertas, Dictionary<string, double> cantidadTipoProductos)
    {
        List<Oferta> ofertas = new List<Oferta>();
        List<string> tipoproductos = new List<string>(cantidadTipoProductos.Keys);

        for (int i = 0; i < cantidadOfertas; i++)
        {
            ofertas.Add(new Oferta()
            {
                IdPublicacion = i + 1,
                IdTipoProducto = i + 1,
                TipoProducto = tipoproductos[i],
                Marca = "Marca" + i + 1,
                Localidad = "Localidad" + i + 1,
                Latitud = LATITUDUNO + cantidadOfertas,
                Longitud = LONGITUDUNO + cantidadOfertas,
                IdLocalidad = i + 1,
                NombreComercio = "Comercio" + i + 1,
                NombreProducto = "Producto" + i + 1,
                Imagen = "Imagen" + i + 1,
                FechaVencimiento = fechaPrueba.ToString("dd-MM-yy"),
                Peso = i + 1,
                Precio = i + 1,
                Unidades = i + 1
            });
        }
        return ofertas;
    }

    private List<int> ComerciosDentroDelRadio(Filtro filtros)
    {
        List<int> idComercios = new List<int>() { 10, 12, 13, 24 };

        comercioServicio.Setup(c => c.ObtenerComerciosPorRadio(filtros.LatitudUbicacion, filtros.LongitudUbicacion, filtros.Distancia))
                        .Returns(idComercios);
        return idComercios;
    }

    private List<int> IdTipoProductos(Filtro filtros)
    {
        List<int> idProductos = new List<int>();
        List<int> idBebidas = new List<int>() { 15, 17 };
        List<int> idComidas = new List<int>() { 33, 36, 37 };
        tipoProductoServicio.Setup(b => b.ObtenerTiposDeBebida(filtros.Bebidas)).Returns(idBebidas);
        tipoProductoServicio.Setup(b => b.ObtenerTiposDeComida(filtros.Comidas)).Returns(idComidas);
        idProductos.AddRange(idBebidas);
        idProductos.AddRange(idComidas);

        return idProductos;
    }

    private List<string> MarcasElegidas(List<int> idBebidas, List<int> idComidas)
    {
        List<string> marcas = new List<string>();
        List<string> marcasBebida = new List<string>() { "Coca-cola", "Citric" };
        List<string> marcasComida = new List<string>() { "Pureza", "Marolio", "Doña Aurora", "Blanca Flor" };
        ofertaRepo.Setup(m => m.ObtenerMarcasBebidasDisponibles(idBebidas)).Returns(marcasBebida);
        ofertaRepo.Setup(m => m.ObtenerMarcasComidasDisponibles(idComidas)).Returns(marcasComida);
        marcas.AddRange(marcasBebida);
        marcas.AddRange(marcasComida);

        return marcas;
    }

    private List<Oferta> OfertasEncontradas(List<int> idComercios, List<int> idProductos, List<string> marcas, Dictionary<string, double> cantidadTipoProductos)
    {
        List<Oferta> ofertasEncontradas = SetDeOfertasEjemplo(2, cantidadTipoProductos);
        ofertaRepo.Setup(o => o.OfertasDentroDelRadioV2(idProductos, idComercios, marcas)).Returns(ofertasEncontradas);

        return ofertasEncontradas;
    }*/
}