using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Entities.DTOs
{
    public class OfertaDTO
    {
        public string NombreProducto { get; set; } = null!;

        public string Marca { get; set; } = null!;

        public string Imagen { get; set; } = null!;

        public decimal Precio { get; set; } 

        public string NombreComercio { get; set; } = null!;
    }
}
