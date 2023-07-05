using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListadoOfertasDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult GuardarListado([FromBody]ListadoOfertasDTO listado)
        {
            try
            {
                int idListadoGuardado = _listadoOfertasService.GuardarListadoConOfertas(listado);

                if (idListadoGuardado != 0)
                {
                    return Ok(new { message = "Se guardó con éxito el listado seleccionado. Id: "+$"{idListadoGuardado}"});
                }
                else
                {
                    return BadRequest(new {message = "Error al guardar el listado"});
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("detalleListado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListadoOfertasUsuario))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult VerDetalleListado([Required]int idListado, [Required]int idUsuario)
        {
            try
            {
                ListadoOfertasUsuario listado = _listadoOfertasService.BuscarListado(idListado, idUsuario);
                return listado!=null ? Ok(listado) : NoContent();
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("misListados")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ListadosUsuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult VerListados([Required]int idUsuario)
        {
            try
            {
                List<ListadosUsuario> listadosAsociados = _listadoOfertasService.ConsultarListados(idUsuario);
                return listadosAsociados.Count!=0 ? Ok(listadosAsociados) : NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
