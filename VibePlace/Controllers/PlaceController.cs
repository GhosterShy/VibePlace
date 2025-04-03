using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Migrations;
using VibePlace.Models;


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
			var services = await _context.services.ToListAsync();
			var model = new PlaceToService
			{
				places = new Places(), 
				services = services 
			};


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(PlaceToService placeToService, IFormFile image, List<int> selectedServices)
		//public async Task<IActionResult> Create(Places place)
		{

			if (selectedServices == null || selectedServices.Count == 0)
			{
				ModelState.AddModelError("", "Заполните все поля и выберите хотя бы один сервис.");
				return View();
			}
			if (ModelState.ContainsKey("services"))
			{
				ModelState.Remove("services");  
			}



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


					placeToService.places.Image = Path.Combine("img", fileName); // Путь относительно wwwroot
				}
				placeToService.places.UserId = userId;
				_context.places.Add(placeToService.places);
				await _context.SaveChangesAsync();

				

				foreach (var serviceId in selectedServices)
				{
					var serviceplace = new ServiceToPlace
					{
						PlaceId = placeToService.places.Id,
						ServisId = serviceId
					};
					_context.serviceToPlace.Add(serviceplace);
				}

				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Home"); 
			}

			return RedirectToAction("Index","Home");
		}


	}
}
