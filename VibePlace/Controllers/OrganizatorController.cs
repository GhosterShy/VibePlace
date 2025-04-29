using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibePlace.Data;

namespace VibePlace.Controllers
{
	public class OrganizatorController : Controller
	{

		private readonly ILogger<HomeController> _logger;
		private readonly AppIdentityDBContext _context;


		public OrganizatorController(ILogger<HomeController> logger, AppIdentityDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[Authorize(Roles = "Organizator")]
		public async Task<IActionResult> Index()
		{

			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var myPlaces = await _context.places
			   .Where(p => p.UserId == userId)
			   .ToListAsync();


			return View(myPlaces);
		}
	}
}
