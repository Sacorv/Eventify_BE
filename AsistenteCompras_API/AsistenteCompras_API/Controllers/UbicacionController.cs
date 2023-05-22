using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Services;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UbicacionController : ControllerBase
{
    private IUbicacionService _service;

	public UbicacionController(IUbicacionService service)
	{
		_service = service;
	}

	[HttpGet("localidades")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LocalidadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]

	public IActionResult ObtenerLocalidades()
	{
		return Ok(_service.ObtenerTodasLasLocalidades());
	}
}
