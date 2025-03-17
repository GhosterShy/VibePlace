using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Models;
using static System.Formats.Asn1.AsnWriter;

namespace VibePlace.Controllers
{



    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly AppIdentityDBContext _context;


		public HomeController(ILogger<HomeController> logger,AppIdentityDBContext context)
		{
			_context = context;
			_logger = logger;
		}

		




		public async Task<IActionResult> Index()
		{
			var model = new PlacesCategoryModel
			{
				Places = await _context.places.ToListAsync(),
				Categories = await _context.categories.ToListAsync()
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetPlaces()
		{
			var places = await _context.places.ToListAsync();
			return Ok(places); // Вернёт JSON-ответ
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var categories = await _context.categories.ToListAsync();
			return Ok(categories); 
		}










		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
