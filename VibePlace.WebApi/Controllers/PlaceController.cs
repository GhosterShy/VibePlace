using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using VibePlace.WebApi.Models;
using VibePlace.WebApi.Models_for;

namespace VibePlace.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {


		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;


		public PlaceController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}



		[HttpGet]
		[Route("places")] 
		public async Task<ActionResult<Places>> GetPlacesWithCategories()
		{

			var places = await _context.places.ToListAsync();
			

			return Ok(places);
		}



		[HttpGet("place-info")]
		public async Task<ActionResult<PlaceToService>> GetCreateData()
		{
			var services = await _context.services.ToListAsync();

			var dto = new PlaceToService
			{
				places = new Places(), 
				services = services,
			
			};

			return Ok(dto);
		}



		[HttpGet]
		[Route("info/{id:int}")]
		public async Task<ActionResult<Places?>> GetPlaceByIdAsync(int id)
		{
			var place = await _context.places
				.Include(p => p.Images)
				.Include(r => r.Reviews)
					.ThenInclude(u => u.User)
				.Include(c => c.Category)
				.Include(w => w.Ratings)
				.Include(s => s.ServiceToPlaces)
				.AsSplitQuery()
				.FirstOrDefaultAsync(i => i.Id == id);

			return Ok(place);

		}


		[HttpGet]
		[Route("FilterPlace/{categoryId:int}")]
		public async Task<ActionResult<Places>> FilterPlaces(int categoryId)
		{
			

			var filteredPlaces = await _context.places
				.Where(p => p.CategoryId == categoryId)
				.ToListAsync();

			return Ok(filteredPlaces);




		}







		





	}
}
