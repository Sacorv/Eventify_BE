using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Entities.DTOs
{
    public class OfertasDTO
    {
        public int IdPublicacion { get; set; }

        public int IdTipoProducto { get; set; }

        public string NombreProducto { get; set; } = null!;

        public string Marca { get; set; } = null!;

        public string Imagen { get; set; } = null!;

        public decimal Precio { get; set; }

        public string NombreComercio { get; set; } = null!;

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
    }
}

