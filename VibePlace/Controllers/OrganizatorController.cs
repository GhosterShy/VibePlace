using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VibePlace.Data;
using VibePlace.Data.Models;

namespace VibePlace.Controllers
{
	public class OrganizatorController : Controller
	{

		private readonly ILogger<HomeController> _logger;
		private readonly AppIdentityDBContext _context;


		public OrganizatorController(ILogger<HomeController> logger, AppIdentityDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[Authorize(Roles = "Organizator")]
		public async Task<IActionResult> Index()
		{
			var myPlaces = new List<Places>();
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Login", "Account");
			}

			//var myPlaces = await _context.places
			//   .Where(p => p.UserId == userId)
			//   .ToListAsync();



			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);


				using (var PlaceResponse = await client.GetAsync($"http://api.mukha.satbayevproject.kz/api/Place/Organizator/" + userId))
				{
					var serviceResult = await PlaceResponse.Content.ReadAsStringAsync();
					myPlaces = JsonConvert.DeserializeObject<List<Places>>(serviceResult);
				}
			}

				return View(myPlaces);
		}









	}
}
