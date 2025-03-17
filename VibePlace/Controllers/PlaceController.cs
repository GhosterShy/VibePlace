using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VibePlace.Data;
using VibePlace.Data.Models;


namespace VibePlace.Controllers
{
	public class PlaceController : Controller
	{
		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;


		public PlaceController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}

		public async  Task<IActionResult> Create()
		{
			ViewBag.Categories = await _context.categories.ToListAsync();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Places place, IFormFile image)
		//public async Task<IActionResult> Create(Places place)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (ModelState.IsValid)
			{

				if (image != null)
				{

					var fileName = Path.GetFileName(image.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);


					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await image.CopyToAsync(stream);
					}


					place.Image = Path.Combine("img", fileName); // Путь относительно wwwroot
				}
				place.UserId = userId;
		
				_context.places.Add(place); 
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Home"); 
			}

			return View(place);
		}


	}
}
