using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Moq;

namespace AsistenteCompras_Tests.Service;

public class OfertaServiceTest
{
    private static Mock<IOfertaRepository> ofertaRepo = new Mock<IOfertaRepository>();
    
    private static Mock<IComercioService> comercioServicio = new Mock<IComercioService>();

    private static Mock<ITipoProductoService> tipoProductoServicio = new Mock<ITipoProductoService>();
    
    private static Mock<IUbicacionService> ubicacionServicio = new Mock<IUbicacionService>();

    private DateTime fechaArgentina = DateTime.UtcNow.AddHours(-3);

    private OfertaService ofertaServicio = new OfertaService(ofertaRepo.Object, comercioServicio.Object, tipoProductoServicio.Object, ubicacionServicio.Object);

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

        List<OfertaDTO> ofertasDelComercio = new List<OfertaDTO>() {
            new OfertaDTO
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

        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(1,fechaArgentina)).Returns(ofertasDelComercio);

        var resultado = ofertaServicio.ListaRecorrerMenos(filtro);

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
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, {"gaseosa", 1 } }
        };

        List<int> comerciosEncontados = new List<int>() { 1,2 };

        List<OfertaDTO> ofertasDelComercioChino = new List<OfertaDTO>() {
            new OfertaDTO
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

        List<OfertaDTO> ofertasDelComercioAlmacen = new List<OfertaDTO>() {
            new OfertaDTO
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
            new OfertaDTO
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
            new OfertaDTO
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

        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0],fechaArgentina)).Returns(ofertasDelComercioChino);
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1],fechaArgentina)).Returns(ofertasDelComercioAlmacen);
        
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[0])).Returns("ImagenChino");
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[1])).Returns("ImagenAlmacen");

        var resultado = ofertaServicio.ListaRecorrerMenos(filtro);

        Assert.Equal("Almacen",resultado.First().NombreComercio);
    }

}