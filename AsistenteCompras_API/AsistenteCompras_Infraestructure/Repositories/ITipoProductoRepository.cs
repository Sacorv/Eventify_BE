namespace AsistenteCompras_Infraestructure.Repositories;

public interface ITipoProductoRepository
{
    List<int> ObtenerIdTipoProductosComida(int idComida);
    List<int> ObtenerIdTipoProductosBebida(int idBebida);
}
