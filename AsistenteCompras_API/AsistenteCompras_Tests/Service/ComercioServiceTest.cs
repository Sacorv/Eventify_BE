using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using Moq;

namespace AsistenteCompras_Tests.Service;

public class ComercioServiceTest
{
    private static Mock<IComercioRepository> comercioRepo = new Mock<IComercioRepository>();
    private static Mock<IOfertaRepository> ofertaRepo = new Mock<IOfertaRepository>();

    private ComercioService comercioServicio = new ComercioService(comercioRepo.Object,ofertaRepo.Object);

    [Fact]
    public void QueSeleccioneLaOfertaMasCercanaSegunMiUbicacion()
    {
        DadoLasSiguientesOfertasEnMiUbicacion(out double latitudDeMiUbiacion, out double longitudDeMiUbiacion, out Oferta ofertaCercana, out Oferta ofertaLejana);
        Oferta recomendacion = CuandoHayMasDeUnaOferta(latitudDeMiUbiacion, longitudDeMiUbiacion, ofertaCercana, ofertaLejana);
        EntoncesRecomendarLaMasCercana(recomendacion);

    }

    private static void EntoncesRecomendarLaMasCercana(Oferta recomendacion)
    {
        Assert.Equal("Almacen", recomendacion.NombreComercio);
    }

    private Oferta CuandoHayMasDeUnaOferta(double latitudDeMiUbiacion, double longitudDeMiUbiacion, Oferta ofertaCercana, Oferta ofertaLejana)
    {
        return comercioServicio.CompararDistanciaEntreComercios(latitudDeMiUbiacion, longitudDeMiUbiacion, ofertaCercana, ofertaLejana);
    }

    private static void DadoLasSiguientesOfertasEnMiUbicacion(out double latitudDeMiUbiacion, out double longitudDeMiUbiacion, out Oferta ofertaCercana, out Oferta ofertaLejana)
    {
        latitudDeMiUbiacion = -34.68550;
        longitudDeMiUbiacion = -58.50166;
        ofertaLejana = new Oferta()
        {
            IdPublicacion = 2,
            IdTipoProducto = 3,
            TipoProducto = "Salchicha",
            NombreProducto = "Salchicha vienissima 12 unidades",
            Marca = "vienissima",
            Imagen = "imagen",
            Precio = 100,
            Peso = 35,
            Unidades = 1,
            NombreComercio = "Chino",
            Localidad = "Madero",
            IdLocalidad = 1,
            Latitud = -34.68495,
            Longitud = -58.50218,
        };
        ofertaCercana = new Oferta()
        {
            IdPublicacion = 2,
            IdTipoProducto = 3,
            TipoProducto = "Salchicha",
            NombreProducto = "Salchicha vienissima 12 unidades",
            Marca = "vienissima",
            Imagen = "imagen",
            Precio = 100,
            Peso = 35,
            Unidades = 1,
            NombreComercio = "Almacen",
            Localidad = "Madero",
            IdLocalidad = 1,
            Latitud = -34.68521,
            Longitud = -58.50171,
        };
    }
}
