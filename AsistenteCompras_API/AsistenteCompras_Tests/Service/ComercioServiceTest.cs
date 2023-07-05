using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
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
        DadoLasSiguientesOfertasEnMiUbicacion(out double latitudDeMiUbiacion, out double longitudDeMiUbiacion, out OfertaDTO ofertaCercana, out OfertaDTO ofertaLejana);
        OfertaDTO recomendacion = CuandoHayMasDeUnaOferta(latitudDeMiUbiacion, longitudDeMiUbiacion, ofertaCercana, ofertaLejana);
        EntoncesRecomendarLaMasCercana(recomendacion);

    }

    private static void EntoncesRecomendarLaMasCercana(OfertaDTO recomendacion)
    {
        Assert.Equal("Almacen", recomendacion.NombreComercio);
    }

    private OfertaDTO CuandoHayMasDeUnaOferta(double latitudDeMiUbiacion, double longitudDeMiUbiacion, OfertaDTO ofertaCercana, OfertaDTO ofertaLejana)
    {
        return comercioServicio.CompararDistanciaEntreComercios(latitudDeMiUbiacion, longitudDeMiUbiacion, ofertaCercana, ofertaLejana);
    }

    private static void DadoLasSiguientesOfertasEnMiUbicacion(out double latitudDeMiUbiacion, out double longitudDeMiUbiacion, out OfertaDTO ofertaCercana, out OfertaDTO ofertaLejana)
    {
        latitudDeMiUbiacion = -34.68550;
        longitudDeMiUbiacion = -58.50166;
        ofertaLejana = new OfertaDTO()
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
        ofertaCercana = new OfertaDTO()
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
