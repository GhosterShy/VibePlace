using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			var place = await _placeService.GetPlaceByIdAsync(id);
			var servise = await _placeService.GetServiceByIdAsync(id);

			if (place == null)
			{
				return NotFound("Место с указанным ID не найдено.");
			}
			var model = new PlaceToService
			{
				places = place,
				services = servise
			};

			Console.WriteLine($"Отзывы загружены: {place.Reviews?.Count ?? 0}");

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
				else
				{
					Review review = new Review
					{
						UserId = userId,
						Comment = Comment,
						PlaceId = PlaceId,
						CreatedAt = DateTime.Now
					};
					_context.review.Add(review);
					await _context.SaveChangesAsync();
				}
			}

			return RedirectToAction("placeinfo", "PlaceInfo", new { id = PlaceId });
		}








	}
}
