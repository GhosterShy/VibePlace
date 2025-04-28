using Microsoft.AspNetCore.Mvc;

namespace VibePlace.Admin.Controllers
{
	public class UsersController : Controller
	{
		public async Task<IActionResult> Users()
		{
			return View();
		}
	}
}
