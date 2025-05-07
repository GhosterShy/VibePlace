using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class ServiceController : Controller
	{
		private readonly ILogger<PlaceController> _logger;
		private readonly AppIdentityDbContext _context;
		private HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }


		public ServiceController(ILogger<PlaceController> logger,AppIdentityDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async  Task<IActionResult> Service()
		{
			List<Service> services = new List<Service>();
			using (var responce = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Service/service"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				services = JsonConvert.DeserializeObject<List<Service>>(result);
			}

			return View(services);
		}

		public IActionResult CreateService()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateService(Service service)
		{
			if (ModelState.IsValid)
			{
				_context.services.Add(service);
				await _context.SaveChangesAsync();
				return RedirectToAction("Service", "Service");
			}

			return View(service);
		}
	}
}
