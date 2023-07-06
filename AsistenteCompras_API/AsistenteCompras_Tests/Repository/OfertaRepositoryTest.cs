using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Infraestructure.Contexts;
using AsistenteCompras_API.Infraestructure.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace AsistenteCompras_Tests.Repository
{
    public class OfertaRepositoryTest
    {

        private Localidad SanJusto = new Localidad() { Nombre = "San Justo" };
        private Localidad RamosMejia = new Localidad() { Nombre = "Ramos mejía" };
        private List<int> idProductos = new List<int> { 1, 2, 3, 4, 5 };
        private DateTime fechaArgentina = DateTime.UtcNow.AddHours(-3).Date;

        private AsistenteComprasContext _context;
        private OfertaRepository _ofertaRepository;


        public OfertaRepositoryTest()
        {
            _context = ContextMemory.Generate();
            _ofertaRepository = new OfertaRepository(_context);
        }


        [Fact]
        public void QuePuedaObtenerLasOfertasDeProductosSegunLocalidadYIdsTipoProductosDeBebidaYComida()
        {
            DadoQueExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(SanJusto, 3);
            DadoQueExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(RamosMejia, 2);

            List<OfertaDTO> ofertas = CuandoBuscoOfertasDeDeterminadosProductosPorLocalidad(SanJusto, idProductos);

            EntoncesEncuentroLasOfertas(3, ofertas);
        }


        private void DadoQueExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(Localidad localidad, int cantidadOfertas)
        {
            _context.Add(localidad);
            _context.SaveChanges();

            Comercio comercio = new Comercio();
            comercio.IdLocalidad = localidad.Id;
            comercio.RazonSocial = "Mounstro Mercado";
            comercio.Direccion = "Avenida siempre viva 1050";
            comercio.Clave = "contraseña123";
            comercio.CUIT = "12345";
            comercio.Email = "comerciocorreo@hotmail.com";
            comercio.Imagen = "imagenComercio";


            _context.Add(comercio);
            _context.SaveChanges();

            Categorium categoria = new Categorium();
            categoria.Nombre = "bebidas";
            categoria.Estado = true;
            _context.Add(categoria);
            _context.SaveChanges();

            TipoProducto tipoProducto1 = new TipoProducto();
            TipoProducto tipoProducto2 = new TipoProducto();
            TipoProducto tipoProducto3 = new TipoProducto();

            tipoProducto1.Nombre = "Vino";
            tipoProducto2.Nombre = "Cerveza";
            tipoProducto3.Nombre = "Gaseosa";

            _context.Add(tipoProducto1);
            _context.Add(tipoProducto2);
            _context.Add(tipoProducto3);
            _context.SaveChanges();

            List<Producto> productos = new List<Producto>
            {
                new Producto
                {
                    Nombre = "Vino Lopez",
                    Marca = "Lopez",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto1.Id,
                    Imagen = "imagenVino",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "123",
                },
                new Producto
                {
                    Nombre = "Agua Ivess 1L",
                    Marca = "Ivess",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto3.Id,
                    Imagen = "imagenAgua",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "125"
                },
                new Producto
                {
                    Nombre = "Heineken 1L",
                    Marca = "Heineken",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto2.Id,
                    Imagen = "imagenCerveza",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "124"
                }
            };

            for (int i=0; i<cantidadOfertas; i++)
            {
                _context.Add(productos[i]);
                _context.SaveChanges();

                Publicacion publicacionNueva = new Publicacion();
                publicacionNueva.IdComercio = comercio.Id;
                publicacionNueva.IdProducto = productos[i].Id;
                publicacionNueva.Estado = true;
                publicacionNueva.Precio = 100;
                publicacionNueva.FechaFin = DateTime.UtcNow;

                _context.Add(publicacionNueva);
                _context.SaveChanges();
            }
        }

        private List<OfertaDTO> CuandoBuscoOfertasDeDeterminadosProductosPorLocalidad(Localidad localidad, List<int> idProductos)
        {
            return _ofertaRepository.OfertasPorLocalidad(localidad.Id, idProductos);
        }

        private void EntoncesEncuentroLasOfertas(int cantidadOfertas, List<OfertaDTO> ofertas)
        {
            Assert.Equal(cantidadOfertas, ofertas.Count);
        }

        [Fact]
        public void QuePuedaObtenerOfertasDeProductosSegunRadioDeDistanciaYMarcasDeBebidaYComidaElegidas()
        {
            
        }

        //[Fact]
        public void QuePuedaObtenerLasOfertasDeComercioConFechaMayor()
        {
            List<DateTime> fechas = new List<DateTime>()
            {
                new DateTime(2023,07,05),
                new DateTime(2023,07,04),
                new DateTime(2023,07,12)
            };

            var comercio = DadoQueExistenOfertasDeUnComercioConLaSiguientesFechas(fechas);

            var resultado = _ofertaRepository.OfertasPorComercioFiltradasPorFecha(comercio, fechaArgentina);

            

            Assert.Equal("05-07-23", resultado.First().FechaVencimiento);
            Assert.Equal(2, resultado.Count);
            
            
        }

        private int DadoQueExistenOfertasDeUnComercioConLaSiguientesFechas(List<DateTime> fechas)
        {
            _context.Add(SanJusto);
            _context.SaveChanges();

            Comercio comercio = new Comercio();
            comercio.IdLocalidad = SanJusto.Id;
            comercio.RazonSocial = "Mounstro Mercado";
            comercio.Direccion = "Avenida siempre viva 1050";
            comercio.Clave = "contraseña123";
            comercio.CUIT = "12345";
            comercio.Email = "comerciocorreo@hotmail.com";
            comercio.Imagen = "imagenComercio";


            _context.Add(comercio);
            _context.SaveChanges();

            Categorium categoria = new Categorium();
            categoria.Nombre = "bebidas";
            categoria.Estado = true;
            _context.Add(categoria);
            _context.SaveChanges();

            TipoProducto tipoProducto1 = new TipoProducto();
            TipoProducto tipoProducto2 = new TipoProducto();
            TipoProducto tipoProducto3 = new TipoProducto();

            tipoProducto1.Nombre = "Vino";
            tipoProducto2.Nombre = "Cerveza";
            tipoProducto3.Nombre = "Gaseosa";

            _context.Add(tipoProducto1);
            _context.Add(tipoProducto2);
            _context.Add(tipoProducto3);
            _context.SaveChanges();

            List<Producto> productos = new List<Producto>
            {
                new Producto
                {
                    Nombre = "Vino Lopez",
                    Marca = "Lopez",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto1.Id,
                    Imagen = "imagenVino",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "123",
                },
                new Producto
                {
                    Nombre = "Agua Ivess 1L",
                    Marca = "Ivess",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto3.Id,
                    Imagen = "imagenAgua",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "125"
                },
                new Producto
                {
                    Nombre = "Heineken 1L",
                    Marca = "Heineken",
                    Estado = true,
                    IdCategoria = categoria.Id,
                    IdTipoProducto = tipoProducto2.Id,
                    Imagen = "imagenCerveza",
                    Peso = 0,
                    Unidades = 0,
                    CodigoBarras = "124"
                }
            };

            for (int i = 0; i < fechas.Count; i++)
            {
                _context.Add(productos[i]);
                _context.SaveChanges();

                Publicacion publicacionNueva = new Publicacion();
                publicacionNueva.IdComercio = comercio.Id;
                publicacionNueva.IdProducto = productos[i].Id;
                publicacionNueva.Estado = true;
                publicacionNueva.Precio = 100;
                publicacionNueva.FechaFin = fechas[i];

                _context.Add(publicacionNueva);
                _context.SaveChanges();
            }

            return comercio.Id;
        }

    }
}
