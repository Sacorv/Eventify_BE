using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;

namespace AsistenteCompras_API.Domain.Services;

public class VerificadorComercioService : IVerificadorComercioService
{
    private readonly IConfiguration _configuration;

    public VerificadorComercioService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> VerificarComercioPorCuit(string cuit)
    {
        string comercioVerificado = "";

        BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetValue<string>("ConnectionStrings:BlobStorage"));
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("csv");
        BlobClient blobClient = containerClient.GetBlobClient("registro-nacional-sociedades-202212.csv");
        string separador = ",";

        if(await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(response.Value.Content))
            {
                while (!streamReader.EndOfStream)
                {
                    var registroCSV = await streamReader.ReadLineAsync();
                    if (!registroCSV.IsNullOrEmpty())
                    {
                        string[] registroSeparado = registroCSV.Split(separador);
                        if (registroSeparado[0].Equals(cuit))
                        {
                            comercioVerificado = registroSeparado[0] + " " + registroSeparado[1];
                            break;
                        }
                    }
                }
            }
        } 
        
        return comercioVerificado;
    }
}
