using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibePlace.Data;
using VibePlace.Data.Models;

namespace VibePlace.Controllers
{
    public class ServiceController : Controller
    {

		private readonly AppIdentityDBContext _context;


		public ServiceController(AppIdentityDBContext context)
		{
			_context = context;
		}


		[Route("Services")]
		public IActionResult Service()
		{
			return View();
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
