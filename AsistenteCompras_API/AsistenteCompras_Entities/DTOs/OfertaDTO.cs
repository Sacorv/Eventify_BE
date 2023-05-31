namespace AsistenteCompras_Entities.DTOs;

public class OfertaDTO
{
    public int IdPublicacion { get; set; }

    public int IdTipoProducto { get; set; }

    public int IdLocalidad { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public decimal Precio { get; set; }

    public string NombreComercio { get; set; } = null!;

    public double Latitud { get; set; }

    public double Longitud { get; set; }
    
    public string Localidad { get; set; } = null!;
}

