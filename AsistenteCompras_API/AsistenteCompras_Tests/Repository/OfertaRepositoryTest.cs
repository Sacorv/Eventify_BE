using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Contexts;
using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Tests.Repository
{
    public class OfertaRepositoryTest
    {

        Localidad SanJusto = new Localidad() { Nombre = "San Justo" };
        Localidad RamosMejia = new Localidad() { Nombre = "Ramos mejía" };
        List<int> idProductos = new List<int> { 1, 2, 3, 4, 5 };

        public AsistenteComprasContext _context;
        public OfertaRepository _ofertaRepository;


        public OfertaRepositoryTest()
        {
            _context = ContextMemory.Generate();
            _ofertaRepository = new OfertaRepository(_context);
        }


        //[Fact]
        public void quePuedaObtenerLasOfertasDeProductosSegunLocalidadYIdsTipoProductosDeBebidaYComida()
        {
            givenExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(SanJusto, 3);
            givenExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(RamosMejia, 2);

            List<OfertaDTO> ofertas = whenBuscoOfertasDeDeterminadosProductosPorLocalidad(SanJusto, idProductos);

            thenEncuentroLasOfertas(3, ofertas);
        }


        private void givenExistenOfertasDeComerciosQuePertenecenAUnaLocalidad(Localidad localidad, int cantidadOfertas)
        {
            _context.Add(localidad);
            _context.SaveChanges();

            Comercio comercio = new Comercio();
            comercio.IdLocalidad = localidad.Id;
            comercio.RazonSocial = "Mounstro Mercado";
            comercio.Direccion = "Avenida siempre viva 1050";

            _context.Add(comercio);
            _context.SaveChanges();

            for(int i=0; i<cantidadOfertas; i++)
            {
                Producto producto = new Producto();
                producto.Nombre = "Producto "+i;
                producto.Marca = "Marca" + i;
                producto.Imagen = "";
                producto.IdCategoria = i+1;
                producto.IdTipoProducto = i+1;

                _context.Add(producto);
                _context.SaveChanges();

                Publicacion publicacionNueva = new Publicacion();
                publicacionNueva.IdComercio = comercio.Id;
                publicacionNueva.IdProducto = producto.Id;

                _context.Add(publicacionNueva);
                _context.SaveChanges();
            }
        }

        private List<OfertaDTO> whenBuscoOfertasDeDeterminadosProductosPorLocalidad(Localidad localidad, List<int> idProductos)
        {
            return _ofertaRepository.OfertasPorLocalidad(localidad.Id, idProductos);
        }

        private void thenEncuentroLasOfertas(int cantidadOfertas, List<OfertaDTO> ofertas)
        {
            Assert.Equal(cantidadOfertas, ofertas.Count);
        }



        //[Fact]
        public void quePuedaObtenerOfertasDeProductosSegunRadioDeDistanciaYMarcasDeBebidaYComidaElegidas()
        {
            
        }

    }
}
