using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;

namespace AsistenteCompras_API.Infraestructure.Repositories
{
    public class ListadoOfertasRepository : IListadoOfertasRepository
    {
        private readonly AsistenteComprasContext _context;

        public ListadoOfertasRepository(AsistenteComprasContext context)
        {
            _context = context;
        }


        public ListadoDeOfertas BuscarListado(int idListado)
        {
            return _context.ListadoDeOfertas.FirstOrDefault(l => l.Id == idListado);            
        }

        public int GuardarListado(ListadoDeOfertas listado)
        {
            _context.ListadoDeOfertas.Add(listado);
            _context.SaveChanges();

            return listado.Id;
        }

        public void GuardarOfertaEnListado(OfertaElegida oferta)
        {
            _context.OfertaElegida.Add(oferta);
            _context.SaveChanges();
        }

        public void ModificarListado(ListadoDeOfertas listado)
        {
            throw new NotImplementedException();
        }

        public List<ListadoDeOfertas> ObtenerListados()
        {
            throw new NotImplementedException();
        }
    }
}
