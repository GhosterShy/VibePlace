using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class PlaceController : Controller
	{
		private readonly ILogger<PlaceController> _logger;
		private HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }


		public PlaceController(ILogger<PlaceController> logger)
		{
			_logger = logger;
		}



		public async Task<IActionResult> Index()
		{
			List<Places> places = new List<Places>();
			using (var responce = await client.GetAsync("http://localhost:5292/api/Place/places"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				places = JsonConvert.DeserializeObject<List<Places>>(result);
			}

			return View(places);
		}
	}
}
