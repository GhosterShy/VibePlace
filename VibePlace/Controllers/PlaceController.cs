using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using VibePlace.Data;
using VibePlace.Data.Models;

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
			
			var model = new PlaceToService();
			var city = new List<City>();



			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				model.places = new Places();


				using (var serviceResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Service/service"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					model.services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}

	
				using (var categoriesResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Category/category"))
				{
					var categoriesResult = await categoriesResponse.Content.ReadAsStringAsync();
					ViewBag.Categories = JsonConvert.DeserializeObject<List<Category>>(categoriesResult);
				}

				using (var cityResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/City/cities"))
				{
					var cityResult = await cityResponse.Content.ReadAsStringAsync();
					ViewBag.CityList = JsonConvert.DeserializeObject<List<City>>(cityResult);
				}
				
				
				
		


			}




			return View(model);
		}





		[HttpPost]
		public async Task<IActionResult> Create(PlaceToService placeToService, IFormFile image, List<int> selectedServices, List<IFormFile> photos,string AddressLink)
		//public async Task<IActionResult> Create(Places place)
		{

			var model = new PlaceToService();
			var city = new List<City>();

			if (selectedServices == null || selectedServices.Count == 0)
			{

				

				using (var client = new HttpClient())
				{
					var token = Request.Cookies["token"];

					client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", token);

					model.places = new Places();


					using (var serviceResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Service/service"))
					{
						var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
						model.services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
					}

					using (var categoriesResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Category/category"))
					{
						var categoriesResult = await categoriesResponse.Content.ReadAsStringAsync();
						ViewBag.Categories = JsonConvert.DeserializeObject<List<Category>>(categoriesResult);
					}

					using (var cityResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/City/cities"))
					{
						var cityResult = await cityResponse.Content.ReadAsStringAsync();
						ViewBag.CityList = JsonConvert.DeserializeObject<List<City>>(cityResult);
					}

				}



				ModelState.AddModelError("", "Заполните все поля и выберите хотя бы один сервис.");
				return View(model);
			}


			if (ModelState.ContainsKey("services"))
			{
				ModelState.Remove("services");  
			}



			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (ModelState.IsValid)
			{

				if (image != null && image.Length > 0)
				{
					using (var ms = new MemoryStream())
					{
						await image.CopyToAsync(ms);
						placeToService.places.Image = ms.ToArray();
					}
				}
				placeToService.places.AddressLink = AddressLink;
				placeToService.places.UserId = userId;
				placeToService.places.Rating = 4;
				_context.places.Add(placeToService.places);
				await _context.SaveChangesAsync();



				///Photos
				foreach (var photo in photos)
				{
					if (photo != null && photo.Length > 0)
					{
						
						var placeImage = new PlaceImage();
						
						using (var ms = new MemoryStream())
						{
							await photo.CopyToAsync(ms);
						
							placeImage = new PlaceImage
							{
								PlaceId = placeToService.places.Id,
								ImageUrl = ms.ToArray()

							};
						}


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


			//else



			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				model.places = new Places();


				using (var serviceResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Service/service"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					model.services = JsonConvert.DeserializeObject<List<Service>>(serviceResult);
				}

				using (var categoriesResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Category/category"))
				{
					var categoriesResult = await categoriesResponse.Content.ReadAsStringAsync();
					ViewBag.Categories = JsonConvert.DeserializeObject<List<Category>>(categoriesResult);
				}

				using (var cityResponse = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/City/cities"))
				{
					var cityResult = await cityResponse.Content.ReadAsStringAsync();
					ViewBag.CityList = JsonConvert.DeserializeObject<List<City>>(cityResult);
				}

			}

			return View(model);
		}


		[HttpGet]
		public IActionResult GetImagePlace(int id)
		{
			var place = _context.places.Find(id);
			if (place == null || place.Image == null)
			{
				return NotFound();
			}

			return File(place.Image, "image/jpeg");
		}

		[HttpGet]
		public IActionResult GetImages(int id)
		{
			var images = _context.placeimage.Find(id);
			if (images == null || images.ImageUrl == null)
			{
				return NotFound();
			}

			return File(images.ImageUrl, "image/jpeg");
		}





		[HttpPost]
		public async Task<IActionResult> SearchPlace(string query) 
		{
			var places = new List<Places>();
			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				


				using (var serviceResponse = await client.GetAsync($"http://api.mukha.satbayevproject.kz/api/Place/SearchPlaces/{query}"))
				{
					var serviceResult = await serviceResponse.Content.ReadAsStringAsync();
					places = JsonConvert.DeserializeObject<List<Places>>(serviceResult);
				}
			}

				return View(places);
		}

	}
}
