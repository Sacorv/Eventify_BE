using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IVerificadorComercioService
    {
        string VerificarComercioPorCuit(string cuit);
    }
}
