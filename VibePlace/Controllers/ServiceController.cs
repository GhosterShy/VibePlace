using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VibePlace.Data;
using VibePlace.Data.Models;

namespace VibePlace.Controllers
{
    public class ServiceController : Controller
    {

		private readonly AppIdentityDBContext _context;


		public ServiceController(AppIdentityDBContext context)
		{
			_context = context;
		}


		[Route("Services")]
		public async Task<IActionResult> Service()
		{

			var services = new List<Service>();

			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);


				using (var serviceResponse = await client.GetAsync("http://localhost:5292/api/Service/service"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}
				return View(services);
			}
		}



		public IActionResult CreateService()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> CreateService(Service service)
		{
			_context.services.Add(service);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index", "Home");

		}
	}
}
