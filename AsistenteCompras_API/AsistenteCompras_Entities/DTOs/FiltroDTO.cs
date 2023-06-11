using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Entities.DTOs
{
    public class FiltroDTO
    {
        public double latitudUbicacion { get; set; }
        public double longitudUbicacion { get; set; }
        public float distancia { get; set; }
        public List<int>? comidas { get; set; }
        public List<int>? bebidas { get; set; }
        public List<String>? marcasComida { get; set; }
        public List<String>? marcasBebida { get; set; }
        public Dictionary<string, double>? cantidadProductos { get; set; }

    }
}
