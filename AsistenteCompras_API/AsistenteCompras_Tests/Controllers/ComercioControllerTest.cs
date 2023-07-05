using AsistenteCompras_API.Controllers;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AsistenteCompras_Tests.Controllers;

public class ComercioControllerTest
{
    private static Mock<IComercioService> _comercioService = new Mock<IComercioService>();
    private static Mock<IRolService> _rolService = new Mock<IRolService>();
    private static Mock<IUbicacionService> _ubicacionService = new Mock<IUbicacionService>();
    private static Mock<IProductoService> _productoService = new Mock<IProductoService>();
    private static Mock<IOfertaService> _ofertaService = new Mock<IOfertaService>();
    private ComercioController comercioController;
    private bool comercioExiste;
    private bool productoExiste;
    private bool ofertaExiste;
    private bool errorInterno;

    private OfertaAPublicarDTO oferta = new OfertaAPublicarDTO()
    {
        IdComercio = 1,
        IdProducto = 1,
        Precio = 100,
        FechaFin = DateTime.UtcNow.AddHours(-3)
    };

    public ComercioControllerTest()
    {
        comercioController = new ComercioController(_comercioService.Object, _rolService.Object, _ubicacionService.Object, _productoService.Object, _ofertaService.Object);
    }

    [Fact]
    public void QueCargarOFertaMeDevuelvaUnEstado200()
    {
        comercioExiste = true;
        productoExiste = true;
        ofertaExiste = false;
        errorInterno = false;
        DadoQue(comercioExiste, productoExiste, ofertaExiste, errorInterno);

        int estadoHttpObtenido = CuandoCargaOferta();

        EntoncesObtengoElSiguenteEstadoHttp(200, estadoHttpObtenido);
    }

    [Fact]
    public void QueCargarOfertaMeDevuelvaUnEstado404CuandoElComercioNoExiste()
    {
        comercioExiste = false;
        productoExiste = true;
        ofertaExiste = false;
        errorInterno = false;
        DadoQue(comercioExiste, productoExiste, ofertaExiste, errorInterno);

        int estadoHttpObtenido = CuandoCargaOferta();

        EntoncesObtengoElSiguenteEstadoHttp(404, estadoHttpObtenido);
    }

    [Fact]
    public void QueCargarOfertaMeDevuelvaUnEstado404CuandoElProductoNoExiste()
    {
        comercioExiste = true;
        productoExiste = false;
        ofertaExiste = false;
        errorInterno = false;
        DadoQue(comercioExiste, productoExiste, ofertaExiste, errorInterno);

        int estadoHttpObtenido = CuandoCargaOferta();

        EntoncesObtengoElSiguenteEstadoHttp(404, estadoHttpObtenido);
    }

    [Fact]
    public void QueCargarOfertaMeDevuelvaUnEstado400CuandoYaSeEncuentraPublicadaLaOferta()
    {
        comercioExiste = true;
        productoExiste = true;
        ofertaExiste = true;
        errorInterno = false;
        DadoQue(comercioExiste, productoExiste, ofertaExiste, errorInterno);

        int estadoHttpObtenido = CuandoCargaOferta();

        EntoncesObtengoElSiguenteEstadoHttp(400, estadoHttpObtenido);
    }

    [Fact]
    public void QueCargarOfertaMeDevuelvaUnEstado500CuandoHayUnErrorInterno()
    {
        comercioExiste = true;
        productoExiste = true;
        ofertaExiste = false;
        errorInterno = true;
        DadoQue(comercioExiste, productoExiste, ofertaExiste, errorInterno);

        int estadoHttpObtenido = CuandoCargaOferta();

        EntoncesObtengoElSiguenteEstadoHttp(500, estadoHttpObtenido);
    }

    private void DadoQue(bool comercioExiste, bool productoExiste, bool ofertaExiste, bool errorInterno)
    {
        Exception excepcion = new Exception();
        _comercioService.Setup(c => c.VerficarSiElComercioExiste(oferta.IdComercio)).Returns(comercioExiste);
        _productoService.Setup(p => p.VerificarSiElProductoExiste(oferta.IdProducto)).Returns(productoExiste);
        _ofertaService.Setup(o => o.VerficarSiLaOfertaExiste(oferta.IdComercio, oferta.IdProducto)).Returns(ofertaExiste);
        if (!errorInterno)
        {
            _comercioService.Setup(c => c.CargarOfertaDelComercio(oferta.IdComercio, oferta.IdProducto, oferta.Precio, oferta.FechaFin)).Returns(1);
        }
        else
        {
            _comercioService.Setup(c => c.CargarOfertaDelComercio(oferta.IdComercio, oferta.IdProducto, oferta.Precio, oferta.FechaFin)).Throws(excepcion);
        }
        
    }

    private int CuandoCargaOferta()
    {
        return ((ObjectResult)comercioController.CargarOfertaDelComercio(oferta)).StatusCode.Value;
    }

    private void EntoncesObtengoElSiguenteEstadoHttp(int estadoHttpEsperado, int estadoHttpObtenido)
    {
        Assert.Equal(estadoHttpEsperado, estadoHttpObtenido);
    }

}
