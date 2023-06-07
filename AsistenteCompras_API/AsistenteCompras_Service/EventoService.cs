using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Infraestructure.Repositories;

namespace AsistenteCompras_Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _eventoRepository;

    private readonly IComidaRepository _comidaRepository;

    public EventoService(IEventoRepository eventoRepository, IComidaRepository comidaRepository)
    {
        _eventoRepository = eventoRepository;
        _comidaRepository = comidaRepository;
    }

    public List<Bebidum> ObtenerBebidas(int idEvento)
    {
        return _eventoRepository.ObtenerBebidas(idEvento);
    }

    public List<Comidum> ObtenerComidas(int idEvento)
    {
        return _eventoRepository.ObtenerComidas(idEvento);
    }

    public List<Evento> ObtenerEventos()
    {
        return _eventoRepository.ObtenerEventos();
    }

    public List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida)
    {
        return _eventoRepository.ObtenerListadoParaEvento(idEvento, idComida, idBebida);
    }

    public List<TipoProductoDTO> ObtenerListadoParaEvento(List<int> idComidas, int invitados)
    {
        int minimoDeComensales;
        int cantidadAPreparar; 
        List<TipoProductoDTO> productosAComprar = new List<TipoProductoDTO>();
        List<TipoProductoDTO> ingredientes;

        foreach(int idComida in idComidas)
        {
            ingredientes = _comidaRepository.ObtenerIngredientes(idComida);
            minimoDeComensales = _comidaRepository.ObtenerCantidadMinimaDeComensales(idComida);
            cantidadAPreparar = ObtenerCantidadAPreparar(minimoDeComensales, invitados);

            CalcularCantidadesPorIngrediente(cantidadAPreparar, ingredientes);

            ActualizarProductosAComprar(productosAComprar, ingredientes);

            productosAComprar.AddRange(ingredientes);
        }

        return productosAComprar;
    }

    private static void ActualizarProductosAComprar(List<TipoProductoDTO> productosAComprar, List<TipoProductoDTO> ingredientes)
    {
        TipoProductoDTO? productoEnLista;
        foreach (TipoProductoDTO ingrediente in ingredientes)
        {
            productoEnLista = productosAComprar.Where(p => p.Id == ingrediente.Id).FirstOrDefault();
            if (productoEnLista != null)
            {
                if(ingrediente.Peso != 0)
                {
                    ingrediente.Peso += productoEnLista.Peso;
                    productosAComprar.Remove(productoEnLista);
                }
                else
                {
                    ingrediente.Unidades += productoEnLista.Unidades;
                    productosAComprar.Remove(productoEnLista);
                }
            }
        }
    }

    private static void CalcularCantidadesPorIngrediente(int cantidadAPreparar, List<TipoProductoDTO> ingredientes)
    {
        foreach (TipoProductoDTO ingrediente in ingredientes)
        {
            if (ingrediente.Peso != 0)
            {
                ingrediente.Peso *= cantidadAPreparar;
            }
            else
            {
                ingrediente.Unidades *= cantidadAPreparar;
            }
        }
    }

    private static int ObtenerCantidadAPreparar(int minimoDeComensales, int invitados)
    {
        Decimal resultado = (Decimal)invitados / (Decimal)minimoDeComensales;
        Decimal parteDecimal = (resultado - ((int)resultado)) * 100;
       
        int cantidadAPreparar = ((int)resultado);
        if(parteDecimal > 0) cantidadAPreparar++;

        return cantidadAPreparar;
    }
}