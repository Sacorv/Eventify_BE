using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using Moq;

namespace AsistenteCompras_Tests.Service
{
    public class ListadoOfertasServiceTest
    {
        private static Mock<IListadoOfertasRepository> listadoOfertaRepo = new Mock<IListadoOfertasRepository>();

        private ListadoOfertasService listadoOfertaServicio = new ListadoOfertasService(listadoOfertaRepo.Object);

        private DateTime fechaPrueba = DateTime.UtcNow.AddDays(+30).Date;

        [Fact]
        public void QuePuedaVerListadosDeUnUsuario()
        {
            int dadoUnUsuarioExistente = 1;
            List<ListadosUsuario> listados = CuandoBuscoSusListadosAsociados(dadoUnUsuarioExistente);
            EntoncesObtengoSus(listados);
        }     

        private List<ListadosUsuario> CuandoBuscoSusListadosAsociados(int idUsuarioExistente)
        {
            ListadosPrueba();

            return listadoOfertaServicio.ConsultarListados(idUsuarioExistente);
        }

        private void EntoncesObtengoSus(List<ListadosUsuario> listados)
        {
            Assert.True(listados.Count==2);
        }


        [Fact]
        public void QuePuedaBuscarUnListadoDeUnUsuario()
        {
            int dadoUnUsuarioExistente = 1;
            int IdListadoDelUsuario = 2;
            ListadoOfertasUsuario listado = CuandoBuscoUnListado(dadoUnUsuarioExistente, IdListadoDelUsuario);
            EntoncesObtengoSu(listado);
        }

        private ListadoOfertasUsuario CuandoBuscoUnListado(int idUsuarioExistente, int idListado)
        {
            OfertaPrueba(idUsuarioExistente, idListado);

            OfertasAsociadasAListado(idListado);

            return listadoOfertaServicio.BuscarListado(idUsuarioExistente, idListado);
        }

        private void EntoncesObtengoSu(ListadoOfertasUsuario listado)
        {
            Assert.NotNull(listado);
        }


        private List<ListadosUsuario> ListadosPrueba()
        {
            List<ListadosUsuario> listados = new List<ListadosUsuario>();
            listados.Add(new ListadosUsuario() 
            { 
                IdUsuario = 1, 
                IdListado = 1, 
                Evento = "Cumpleaños", 
                ComidasElegidas = new List<string>(){ "Pizza", "Pancho" }, 
                BebidasElegidas = new List<string>() { "Bebidas sin alcohol" }, 
                CantidadOfertas = 10, 
                FechaCreacion = fechaPrueba.ToString("dd-MM-yy"), 
                TotalListado = 8500 
            });
            
            listados.Add(new ListadosUsuario() 
            { 
                IdUsuario = 1, 
                IdListado = 2, 
                Evento = "Cumpleaños", 
                ComidasElegidas = new List<string>() { "Hmburguesas" }, 
                BebidasElegidas = new List<string>() { "Bebidas sin alcohol" }, 
                CantidadOfertas = 5, 
                FechaCreacion = fechaPrueba.ToString("dd-MM-yy"), 
                TotalListado = 5500 
            });
            
            listadoOfertaRepo.Setup(l => l.ObtenerListados(1)).Returns(listados);

            return listados;
        }

        private void OfertaPrueba(int idUsuarioExistente, int idListado)
        {
            ListadoOfertasUsuario listado = new ListadoOfertasUsuario()
            {
                IdListado = idListado,
                IdUsuario = idUsuarioExistente,
                Evento = "Cumpleaños",
                Ofertas = new List<OfertaCantidad>(),
                ComidasElegidas = new List<string>() { "Hmburguesas" },
                BebidasElegidas = new List<string>() { "Bebidas sin alcohol" },
                CantidadOfertas = 5,
                FechaCreacion = fechaPrueba.ToString("dd-MM-yy"),
                TotalListado = 5500,
                DistanciaARecorrer = 6000,
                MensajeOfertas = "Mejores ofertas",
                UrlRecorrido = "https://ofertas.com",
                Usuario = "Lorena Paola"
            };

            listadoOfertaRepo.Setup(l => l.BuscarListado(idUsuarioExistente, idListado)).Returns(listado);
        }

        private void OfertasAsociadasAListado(int idListado)
        {
            List<OfertaCantidad> ofertasAsociadas = new List<OfertaCantidad>();
            ofertasAsociadas.Add(new OfertaCantidad()
            {
                Cantidad=2,
                Oferta = new Oferta(),
                Subtotal = 2500
            });

            ofertasAsociadas.Add(new OfertaCantidad()
            {
                Cantidad = 3,
                Oferta = new Oferta(),
                Subtotal = 3400
            });

            listadoOfertaRepo.Setup(o => o.BuscarOfertasAsociadas(idListado)).Returns(ofertasAsociadas);
        }
    }
}
