using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class CityController : Controller
	{

		private readonly ILogger<PlaceController> _logger;
		private HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }


		public CityController(ILogger<PlaceController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> City()
		{

			List<City> cities = new List<City>();
			using (var responce = await client.GetAsync("http://localhost:5292/api/City/cities"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				cities = JsonConvert.DeserializeObject<List<City>>(result);
			}
			return View(cities);
		}
	}
}
