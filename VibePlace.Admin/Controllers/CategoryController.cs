using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VibePlace.Admin.Models;

namespace VibePlace.Admin.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ILogger<PlaceController> _logger;
		private readonly AppIdentityDbContext _context;
		private  HttpClient client = new HttpClient();
		public string ApiUrl { get; set; }

		public CategoryController(ILogger<PlaceController> logger, AppIdentityDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async  Task<IActionResult> Category()
		{
			List<Category> categories = new List<Category>();
			using (var responce = await client.GetAsync("http://api.mukha.satbayevproject.kz/api/Category/category"))
			{
				var result = await responce.Content.ReadAsStringAsync();
				categories = JsonConvert.DeserializeObject<List<Category>>(result);
			}

			return View(categories);
		}



		public IActionResult CreateCategory()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCategory(Category category)
		{
			if (ModelState.IsValid)
			{
				_context.categories.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction("Category", "Category");
			}

			return View(category);
		}





	}
}
