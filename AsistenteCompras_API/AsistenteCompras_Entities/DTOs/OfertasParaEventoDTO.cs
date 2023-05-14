using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Entities.DTOs
{
    public class OfertasParaEventoDTO
    {
        public string NombreEvento { get; set; } = null!;

        public virtual ICollection<PublicacionDTO> Productos { get; set; } = new List<PublicacionDTO>();

    }
}
