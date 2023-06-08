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

    public List<TipoProductoDTO> ObtenerListadoParaEvento(ProductosABuscarDTO productosABuscar, int invitados)
    {
        int cantidadMinima;
        int cantidadAComprar; 
        List<TipoProductoDTO> productosEnLista = new List<TipoProductoDTO>();
        List<TipoProductoDTO> productosAcomprar;

        foreach(int idComida in productosABuscar.IdComidas)
        {
            productosAcomprar = _comidaRepository.ObtenerIngredientes(idComida);
            cantidadMinima = _comidaRepository.ObtenerCantidadMinimaDeComensales(idComida);
            cantidadAComprar = ObtenerCantidadAComprar(cantidadMinima, invitados);

            CalcularCantidadesPorProducto(cantidadAComprar, productosAcomprar);

            ActualizarProductosEnLista(productosEnLista, productosAcomprar);

            productosEnLista.AddRange(productosAcomprar);
        }

        foreach(int idBebida in productosABuscar.IdBebidas)
        {
            productosAcomprar = _eventoRepository.ObtenerBebidaTipoProductos(idBebida);
            foreach(TipoProductoDTO producto in productosAcomprar)
            {
                
                cantidadMinima = _eventoRepository.ObtenerCantidadMinimaBebidaPorInvitados(producto.Id);
               
                cantidadAComprar = ObtenerCantidadAComprar(cantidadMinima, invitados);

                CalcularCantidadesPorProducto(cantidadAComprar, producto);

                productosEnLista.Add(producto);
            }
            
        }

        return productosEnLista;
    }

    private static void ActualizarProductosEnLista(List<TipoProductoDTO> productosEnLista, List<TipoProductoDTO> productosAcomprar)
    {
        TipoProductoDTO? productoEncontrado;
        foreach (TipoProductoDTO producto in productosAcomprar)
        {
            productoEncontrado = productosEnLista.Where(p => p.Nombre == producto.Nombre).FirstOrDefault();
            if (productoEncontrado != null)
            {
                if(producto.Peso != 0)
                {
                    producto.Peso += productoEncontrado.Peso;
                    productosEnLista.Remove(productoEncontrado);
                }
                else
                {
                    producto.Unidades += productoEncontrado.Unidades;
                    productosEnLista.Remove(productoEncontrado);
                }
            }
        }
    }

    private static void CalcularCantidadesPorProducto(int cantidadAPreparar, List<TipoProductoDTO> productosAcomprar)
    {
        foreach (TipoProductoDTO producto in productosAcomprar)
        {
            if (producto.Peso != 0)
            {
                producto.Peso *= cantidadAPreparar;
            }
            else
            {
                producto.Unidades *= cantidadAPreparar;
            }
        }
    }

    private static void CalcularCantidadesPorProducto(int cantidadAPreparar, TipoProductoDTO productoAcomprar)
    {
        if (productoAcomprar.Peso != 0)
        {
            productoAcomprar.Peso *= cantidadAPreparar;
        }
        else
        {
            productoAcomprar.Unidades *= cantidadAPreparar;
        }
    }

    private static int ObtenerCantidadAComprar(int cantidadMinima, int invitados)
    {
        Decimal resultado = (Decimal)invitados / (Decimal)cantidadMinima;
        Decimal parteDecimal = (resultado - ((int)resultado)) * 100;
       
        int cantidadAComprar = ((int)resultado);
        if(parteDecimal > 0) cantidadAComprar++;

        return cantidadAComprar;
    }
}