using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Models;

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

			var services = new UserToService();

			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);


				using (var serviceResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Service/service"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					services.Services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}

				using (var userService = await client.GetAsync("http://localhost:5292/api/UserService/userService"))
				{
					var serviceResult = await userService.Content.ReadAsStringAsync();
					services.UsersServices = JsonConvert.DeserializeObject<List<UserService>>(serviceResult);
				}

				//services.UsersServices = _context.UserServices.Include(u => u.User).Include(s => s.Service).ToList();

				return View(services);
			}





		}




		public async Task<IActionResult> FilterService(int serviceId)
		{

			var services = new List<UserService>();

			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);


				if (serviceId == 0000)
				{
					using (var userService = await client.GetAsync("http://localhost:5292/api/UserService/UserService"))
					{
						var serviceResult = await userService.Content.ReadAsStringAsync();
						services = JsonConvert.DeserializeObject<List<UserService>>(serviceResult);
					}
				}
				else
				{
					using (var userService = await client.GetAsync($"http://localhost:5292/api/UserService/FilterService/" + serviceId))
					{
						var serviceResult = await userService.Content.ReadAsStringAsync();
						services = JsonConvert.DeserializeObject<List<UserService>>(serviceResult);
					}
				}

			}


				return PartialView("_ServicesPartial", services);
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
