﻿using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services;

public interface IComercioRepository
{
    Login VerificarComercio(string usuario, string clave);

    Comercio RegistrarComercio(Comercio comercio);

    List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

    string ObtenerImagenComercio(int idComercio);

}
