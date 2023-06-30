using AsistenteCompras_API.Domain.Entities;

namespace AsistenteCompras_API.Domain.Services
{
    public class VerificadorComercioService : IVerificadorComercioService
    {
        public string VerificarComercioPorCuit(string cuit)
        {
            string comercioVerificado = "";

            string pathCsv = "..\\..\\datacsv\\registro-nacional-sociedades-202212.csv";

            System.IO.StreamReader archivo = new StreamReader(pathCsv);
            string separador = ",";
            string registroCsv;

            archivo.ReadLine();

            while((registroCsv = archivo.ReadLine()) != null)
            {
                string[] registro = registroCsv.Split(separador);
                if (registro[0].Equals(cuit))
                {
                    comercioVerificado = registro[0] + " " + registro[1]; 
                    break;
                }
            }
            return comercioVerificado;
        }
    }
}
