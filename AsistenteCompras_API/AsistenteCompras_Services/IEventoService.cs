using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Services
{
    public interface IEventoService
    {
        List<Evento> ObtenerEventos();


    }
}
