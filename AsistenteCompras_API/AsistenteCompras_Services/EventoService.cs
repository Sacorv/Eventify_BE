using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public class EventoService : IEventoService
    {

        private AsistenteComprasContext _context;

        public EventoService(AsistenteComprasContext context)
        {
            this._context = context;
        }

        public List<Evento> ObtenerEventos()
        {
            return this._context.Eventos.ToList();
        }

        

       
    }
}
