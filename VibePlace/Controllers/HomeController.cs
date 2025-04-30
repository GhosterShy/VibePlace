using System.Diagnostics;
using System.Net.Http.Headers;
using System.Numerics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VibePlace.AppFilter;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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



		[IEFilter]
		public async Task<IActionResult> Index()
		{
			var model = new PlacesCategoryModel();

			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				using (var responce = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Place/places"))
				{
					var result = await responce.Content.ReadAsStringAsync();
					model.Places = JsonConvert.DeserializeObject <List<Places>>(result);
				}

		
				using (var categoriesResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Category/category"))
				{
					var categoriesResult = await categoriesResponse.Content.ReadAsStringAsync();
					model.Categories = JsonConvert.DeserializeObject<List<Category>>(categoriesResult);
				}
			}

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


			var places = new List<Places>();

			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];



				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);


				if (categoryId == 0000)
				{
					using (var responce = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Place/places"))
					{
						var placeresult = await responce.Content.ReadAsStringAsync();
						places = JsonConvert.DeserializeObject<List<Places>>(placeresult);
						return PartialView("_PlacesPartial", places);
					}
				}

				using (var responce = await client.GetAsync($"http://api.mukha.satbayevproject.kz/api/Place/FilterPlace/{categoryId}"))
				{
					var result = await responce.Content.ReadAsStringAsync();
					places = JsonConvert.DeserializeObject<List<Places>>(result);
				}


			}

			return PartialView("_PlacesPartial", places);

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
