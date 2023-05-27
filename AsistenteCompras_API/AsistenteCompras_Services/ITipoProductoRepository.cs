namespace AsistenteCompras_Repository;

public interface ITipoProductoRepository
{
    List<int> ObtenerIdTipoProductosComida(int idComida);
    List<int> ObtenerIdTipoProductosBebida(int idBebida);
}
