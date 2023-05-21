using AsistenteCompras_Entities.Entities;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Localidad>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]

	public IActionResult obtenerLocalidades()
	{
		return Ok(_service.ObtenerTodasLasLocalidades());
	}
}
