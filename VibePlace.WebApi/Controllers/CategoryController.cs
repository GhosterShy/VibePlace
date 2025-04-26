using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{

		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;

		public CategoryController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}


		[HttpGet]
		[Route("category")]
		public async Task<ActionResult<Category>> GetCategories()
		{
			var categies = _context.categories.ToList();
			return Ok(categies);
		}

	}
}
