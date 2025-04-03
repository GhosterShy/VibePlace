using Microsoft.AspNetCore.Mvc;
using VibePlace.Data;
using VibePlace.Data.Models;

namespace VibePlace.Controllers
{
    public class CategoryController : Controller
    {
		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;



		public CategoryController(AppIdentityDBContext context)
        {
            _context = context;
        }

		public IActionResult Create()
        {
			return View();
        }


		[HttpPost]
		public async Task<IActionResult> Create(Category category)
        {
            _context.categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateService()
        {
            return View();
        }


		[HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            _context.services.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }


       



    }



}
