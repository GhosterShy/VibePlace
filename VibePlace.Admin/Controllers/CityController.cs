using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class CityController : Controller
	{

		private readonly ILogger<PlaceController> _logger;
		private readonly AppIdentityDbContext _context;
		private HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }


		public CityController(ILogger<PlaceController> logger, AppIdentityDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async Task<IActionResult> City()
		{

			List<City> cities = new List<City>();
			using (var responce = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/City/cities"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				cities = JsonConvert.DeserializeObject<List<City>>(result);
			}
			return View(cities);
		}



		public IActionResult CreateCity()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCity(City city)
		{
			if (ModelState.IsValid)
			{
				_context.cities.Add(city);
				await _context.SaveChangesAsync();
				return RedirectToAction("City","City"); 
			}

			return View(city);
		}




	}
}
