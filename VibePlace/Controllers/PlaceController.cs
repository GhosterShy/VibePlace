using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
			var model = new PlaceToService();



			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				model.places = new Places();


				using (var serviceResponse = await client.GetAsync("http://localhost:5292/api/Service/service"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					model.services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}

				// Загружаем Categories
				using (var categoriesResponse = await client.GetAsync("http://localhost:5292/api/Category/category"))
				{
					var categoriesResult = await categoriesResponse.Content.ReadAsStringAsync();
					ViewBag.Categories = JsonConvert.DeserializeObject<List<Category>>(categoriesResult);
				}
			}




			return View(model);
		}





		[HttpPost]
		public async Task<IActionResult> Create(PlaceToService placeToService, IFormFile image, List<int> selectedServices, List<IFormFile> photos)
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


					placeToService.places.Image = Path.Combine("img", fileName); 
				}

				placeToService.places.UserId = userId;
				placeToService.places.Rating = 4;
				_context.places.Add(placeToService.places);
				await _context.SaveChangesAsync();



				///Photos
				foreach (var photo in photos)
				{
					if (photo.Length > 0)
					{
						var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Images_of_places", photo.FileName);

						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await photo.CopyToAsync(stream);
						}

						var placeImage = new PlaceImage
						{
							PlaceId = placeToService.places.Id,
							ImageUrl = Path.Combine("img", "Images_of_places", photo.FileName)

						};

						_context.placeimage.Add(placeImage);
						await _context.SaveChangesAsync();
					}
				}
				






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
