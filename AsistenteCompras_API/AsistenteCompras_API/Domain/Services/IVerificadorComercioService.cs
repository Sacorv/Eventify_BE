using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IVerificadorComercioService
    {
        Task<string> VerificarComercioPorCuit(string cuit);
    }
}
