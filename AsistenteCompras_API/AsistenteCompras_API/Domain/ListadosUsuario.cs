using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain
{
    public class ListadosUsuario
    {
        public int IdListado { get; set; }
        public int IdUsuario { get; set; }
        public string Evento { get; set; } = null!;
        public List<string> ComidasElegidas { get; set; } = null!;
        public List<string> BebidasElegidas { get; set; } = null!;
        public int CantidadOfertas { get; set; }
        public double TotalListado { get; set; }
        public string FechaCreacion { get; set; } = null!;
    }
}
