using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IOfertaService
{    
    List<Oferta> ObtenerListaProductosEconomicosPorEvento(int idComida, List<int> localidades, int idBebida);

    List<OfertasPorProducto> GenerarListaDeOfertas(Filtro filtro);

    List<OfertasPorComercioDTO> ListaRecorrerMenos(Filtro filtro, List<int> idComercios);

    bool VerficarSiLaOfertaExiste(int idComercio, int idProducto);

}
