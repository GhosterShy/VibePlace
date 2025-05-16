using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using VibePlace.Data;
using VibePlace.Data.Models;

using VibePlace.Models;
using VibePlace.Services;

namespace VibePlace.Controllers
{
	public class PlaceInfoController : Controller
    {
		private PlaceService _placeService;
		private readonly AppIdentityDBContext _context;
		private readonly UserManager<AppUser> _userManager;

		public PlaceInfoController(PlaceService placeService, AppIdentityDBContext context, UserManager<AppUser> userManager)
		{
			_placeService = placeService;
			_context = context;
			_userManager = userManager;
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

				//model.places = await _context.places
				//.Include(p => p.Images)
				//.Include(u => u.User)
				//.Include(r => r.Reviews)
				//	.ThenInclude(u => u.User)
				//.Include(c => c.Category)
				//.Include(w => w.Ratings)
				//.Include(s => s.ServiceToPlaces)
				//.AsSplitQuery()
				//.FirstOrDefaultAsync(i => i.Id == id);




			}




			if (model.places == null)
			{
				return NotFound("Место с указанным ID не найдено.");
			}
			if (model.places == null)
			{
				return NotFound("Место с указанным ID не найдено.");
			}

			if (model.places.Ratings == null)
			{
				Console.WriteLine("Рейтинг не инициализирован");
			}
			else if (model.places.Ratings.Count == 0)
			{
				model.places.Ratings = new List<RatingPlace>();
				Console.WriteLine("Рейтинг загружен, но коллекция пуста");
			}
			else
			{
				Console.WriteLine($"Ретинг загружены: {model.places.Ratings.Count}");
			}

			return View(model);
        }





		//Rating
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Rate(int placeId, int rating)
		{
			var userId = _userManager.GetUserId(User);

			var existingRating = await _context.ratings
				.FirstOrDefaultAsync(r => r.UserId == userId && r.PlaceId == placeId);

			if (existingRating != null)
			{
				existingRating.Rating = rating;
			}
			else
			{
				
				var newRating = new RatingPlace
				{
					PlaceId = placeId,
					UserId = userId,
					Rating = rating
				};

				_context.ratings.Add(newRating);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("PlaceInfo", "PlaceInfo", new { id = placeId });
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
