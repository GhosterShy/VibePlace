using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using VibePlace.WebApi.Models;
using VibePlace.WebApi.Models_for;

namespace VibePlace.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {


		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;


		public PlaceController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}






		

		[HttpGet("create-data")]
		public async Task<ActionResult<PlaceToService>> GetCreateData()
		{
			var services = await _context.services.ToListAsync();

			var dto = new PlaceToService
			{
				places = new Places(), // пустая модель
				services = services,
			
			};

			return Ok(dto);
		}




		[HttpPost("create")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> CreatePlaceApi([FromForm] CreatePlaceDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var place = JsonConvert.DeserializeObject<Places>(model.PlaceJson);

			if (model.SelectedServices == null || model.SelectedServices.Count == 0)
			{
				return BadRequest("Выберите хотя бы один сервис.");
			}

			if (place == null)
			{
				return BadRequest("Невалидные данные места.");
			}

			// Главное изображение
			if (model.Image != null && model.Image.Length > 0)
			{
				var fileName = Path.GetFileName(model.Image.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.Image.CopyToAsync(stream);
				}

				place.Image = Path.Combine("img", fileName);
			}

			place.UserId = userId;
			place.Rating = 4;

			_context.places.Add(place);
			await _context.SaveChangesAsync();

			// Доп. фото
			foreach (var photo in model.Photos)
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
						PlaceId = place.Id,
						ImageUrl = Path.Combine("img", "Images_of_places", photo.FileName)
					};

					_context.placeimage.Add(placeImage);
				}
			}

			// Сервисы
			foreach (var serviceId in model.SelectedServices)
			{
				var serviceplace = new ServiceToPlace
				{
					PlaceId = place.Id,
					ServisId = serviceId
				};
				_context.serviceToPlace.Add(serviceplace);
			}

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(CreatePlaceApi), new { id = place.Id }, place);
		}





	}
}
