using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly AppIdentityDBContext _context;

		public ServiceController(AppIdentityDBContext context)
		{
			_context = context;
		}



		[HttpGet]
		[Route("service")]
		public async Task<ActionResult<Service>> ServiceGet()
		{
			var services = _context.services.ToList();

			return Ok(services);
		}


		[HttpGet]
		[Route("place_ser/{placeId:int}")]
		public async Task<ActionResult<List<Service?>>> GetServiceByIdAsync(int placeId)
		{
			 var service = await _context.services
				.Where(p => p.ServiceToPlaces.Any(s => s.PlaceId == placeId))
				.ToListAsync();

			return Ok(service);
		}

	}
}
