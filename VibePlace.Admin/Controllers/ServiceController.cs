using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class ServiceController : Controller
	{
		private readonly ILogger<PlaceController> _logger;
		private HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }


		public ServiceController(ILogger<PlaceController> logger)
		{
			_logger = logger;
			
		}

		public async  Task<IActionResult> Service()
		{
			List<Service> services = new List<Service>();
			using (var responce = await client.GetAsync("http://localhost:5292/api/Service/service"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				services = JsonConvert.DeserializeObject<List<Service>>(result);
			}

			return View(services);
		}
	}
}
