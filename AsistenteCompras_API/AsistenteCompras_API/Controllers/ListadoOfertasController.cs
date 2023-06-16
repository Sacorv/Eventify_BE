using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GuardarListado(ListadoOfertasDTO listado)
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
    }
}
