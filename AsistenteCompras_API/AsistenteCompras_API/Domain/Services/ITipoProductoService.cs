namespace AsistenteCompras_API.Domain.Services;

public interface ITipoProductoService
{
    List<int> ObtenerIdTipoProductosBebida(int idBebida);
    List<int>  ObtenerIdTipoProductosComida(int idComida);
    List<int> ObtenerTiposDeBebida(List<int> idBebida);
    List<int> ObtenerTiposDeComida(List<int> idComida);
}
