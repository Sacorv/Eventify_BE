﻿using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public interface IUsuarioRepository
    {
        PerfilUsuario VerificarUsuario(string usuario, string clave);

        Usuario RegistrarUsuario(Usuario usuario);
         
    }
}
