using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Migrations;
using VibePlace.Models;
using VibePlace.Services;

namespace VibePlace.Controllers
{
	public class PlaceInfoController : Controller
    {
		private PlaceService _placeService;
		private readonly AppIdentityDBContext _context;

		public PlaceInfoController(PlaceService placeService, AppIdentityDBContext context)
		{
			_placeService = placeService;
			_context = context;
		}



		[Route("place/{id:int}")]
		public async Task<IActionResult> PlaceInfo(int id)
		{
			

			

			var model = new PlaceToService();
			

			/////
			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				


				using (var serviceResponse = await client.GetAsync($"http://api.mukha.satbayevproject.kz/api/Service/place_ser/{id}"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					model.services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}

			
				using (var placesResponse = await client.GetAsync($"http://api.mukha.satbayevproject.kz/api/Place/info/{id}"))
				{
					var placesResult = await placesResponse.Content.ReadAsStringAsync();
					model.places = JsonConvert.DeserializeObject<Places>(placesResult);
				}


			}


			if (model.places == null)
			{
				return NotFound("Место с указанным ID не найдено.");
			}

			Console.WriteLine($"Отзывы загружены: {model.places.Reviews?.Count ?? 0}");

			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> AddReview(String Comment,int PlaceId)
		{

			if (ModelState.IsValid)
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userId == null)
				{
					return RedirectToAction("Register", "Account");
				}

				Review review = new Review
				{
					UserId = userId,
					Comment = Comment,
					PlaceId = PlaceId,
					CreatedAt = DateTime.Now.Date
				};
				_context.review.Add(review);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("placeinfo", "PlaceInfo", new { id = PlaceId });
		}




		[Route("like/{id:int}")]
		public async Task<IActionResult> PlusLike(int id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null || userId == null)
			{
				return RedirectToAction("Register","Account");
			}

			
			
			var likereview = await _context.reviewLike
				.FirstOrDefaultAsync(x => x.UserId == userId && x.ReviewId == id);

			var review = await _context.review.FindAsync(id);
			if (likereview == null)
			{
				
				review.Like += 1;
				await _context.SaveChangesAsync();

				var likereviewadd = new ReviewLike()
				{
					UserId = userId,
					ReviewId = id,
				};
				await _context.reviewLike.AddAsync(likereviewadd);
				await _context.SaveChangesAsync();
				return PartialView("_ReviewPartial", review);
			}



			return PartialView("_ReviewPartial", review);
		}


		public IActionResult Confirmed()
		{
			return View();
		}








	}
}
