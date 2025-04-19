using System.Diagnostics;
using System.Numerics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Models;


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
			return Ok(places); 
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var categories = await _context.categories.ToListAsync();
			return Ok(categories); 
		}


		[HttpGet]
		[Route("/Home/FilterPlaces/{categoryId:int}")]
		public async Task<IActionResult> FilterPlaces(int categoryId)
		{
			if(categoryId==0000)
			{
				var places = await _context.places.ToListAsync();


				return PartialView("_PlacesPartial",places);
			}

			var filteredPlaces = await _context.places
				.Where(p => p.CategoryId == categoryId)
				.ToListAsync();

			return PartialView("_PlacesPartial", filteredPlaces);


			

		}




		[HttpPost]
		public JsonResult ChangeCulture(string culture)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTime.Now.AddMonths(1) });

			return Json(culture);
		}








		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
