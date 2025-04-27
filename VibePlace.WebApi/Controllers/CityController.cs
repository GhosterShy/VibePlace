using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CityController : ControllerBase
	{
		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;

		public CityController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}

		[HttpGet]
		[Route("cities")]
		public async Task<ActionResult<City>> GetCity()
		{
			var cities = _context.cities.ToList();
			return Ok(cities);
		}
	}
}
