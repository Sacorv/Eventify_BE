using AsistenteCompras_API.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificadorComercioController : ControllerBase
    {
        private IVerificadorComercioService _verificadorComercioService;
        public VerificadorComercioController(IVerificadorComercioService verificadorComercioService)
        {
            _verificadorComercioService = verificadorComercioService;
        }

        [HttpGet("verificarComercio")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> VerificarComercioAsync([Required]string cuit)
        {
            try
            {
                var comercioVerificado = await _verificadorComercioService.VerificarComercioPorCuit(cuit);
                if (!comercioVerificado.IsNullOrEmpty())
                {
                    return Ok(new { message = "Comercio verificado por el Registro Nacional de Sociedades: " + $"{comercioVerificado}" });
                }
                else
                {
                    return BadRequest(new { message = "El CUIT ingresado no corresponde a un comercio válido por el Registro Nacional de Sociedades" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

    }
}
