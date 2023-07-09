using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListadoOfertasController : ControllerBase
    {
        private readonly IListadoOfertasService _listadoOfertasService;
        public ListadoOfertasController(IListadoOfertasService listadoOfertasService)
        {
            _listadoOfertasService = listadoOfertasService;
        }

        [HttpPost("guardarListado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public dynamic GuardarListado([FromBody]Listado listado)
        {
            try
            {
                int idListadoGuardado = _listadoOfertasService.GuardarListadoConOfertas(listado);

                if (idListadoGuardado != 0)
                {
                    return new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Se guardó con éxito el listado seleccionado. Id: " + $"{idListadoGuardado}"
                    };
                }
                else
                {
                    return new
                    {
                        statusCode = StatusCodes.Status204NoContent,
                        message = "No se logró guardar el listado"
                    };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("detalleListado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListadoOfertasUsuario))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public dynamic VerDetalleListado([Required]int idListado, [Required]int idUsuario)
        {
            try
            {
                ListadoOfertasUsuario listado = _listadoOfertasService.BuscarListado(idListado, idUsuario);
                return listado!=null ? new { statusCode = StatusCodes.Status200OK, listado} : new { statusCode = StatusCodes.Status204NoContent, message = "El id de listado o usuario incorrecto" };
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("misListados")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ListadosUsuario>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult VerListados([Required]int idUsuario)
        {
            try
            {
                List<ListadosUsuario> listadosAsociados = _listadoOfertasService.ConsultarListados(idUsuario);
                if(listadosAsociados.Count != 0)
                {
                    return Ok(listadosAsociados);
                }
                else
                {
                    return BadRequest("El id de usuario es incorrectos");
                }
                
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
