using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ILogger<PlaceController> _logger;
		private  HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }

		public CategoryController(ILogger<PlaceController> logger)
		{
			_logger = logger;
			
		}

		public async  Task<IActionResult> Category()
		{
			List<Category> categories = new List<Category>();
			using (var responce = await client.GetAsync("http://localhost:5292/api/Category/category"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				categories = JsonConvert.DeserializeObject<List<Category>>(result);
			}

			return View(categories);
		}
	}
}
