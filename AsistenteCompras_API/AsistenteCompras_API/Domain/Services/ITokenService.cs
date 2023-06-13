using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
