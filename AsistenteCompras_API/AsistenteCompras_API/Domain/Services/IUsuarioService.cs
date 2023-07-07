using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IUsuarioService
    {
        PerfilUsuario IniciarSesion(string email, string clave);

        string RegistrarUsuario(Usuario usuario);

        bool ValidarClaves(string clave, string claveAComparar);
    }
}
