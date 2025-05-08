using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using VibePlace.Data;
using VibePlace.Data.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VibePlace.Controllers
{
	public class UserServiceController : Controller
	{
		private readonly AppIdentityDBContext _context;
		private readonly ILogger<UserServiceController> _logger;
		private readonly UserManager<AppUser> _userManager;


		public UserServiceController(AppIdentityDBContext context, ILogger<UserServiceController> logger, UserManager<AppUser> userManager)
		{
			_context = context;
			_logger = logger;
			_userManager = userManager;
		}



		public async Task<IActionResult> CreateServiceUser()
		{
			ViewBag.Services = new SelectList(await _context.services.ToListAsync(), "Id", "Name");
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> CreateServiceUser(UserService model,IFormFile Image)
		{
			if (Image != null && Image.Length > 0)
			{
				using (var ms = new MemoryStream())
				{
					await Image.CopyToAsync(ms);
					model.Image = ms.ToArray();
				}
			}
			if (ModelState.IsValid)
			{
				model.UserId = _userManager.GetUserId(User);
				_context.UserServices.Add(model);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index"); 
			}

			ViewBag.Services = new SelectList(await _context.services.ToListAsync(), "Id", "Name", model.ServiceId);
			return View(model);
		}



		[HttpGet]
		public IActionResult GetImageService(int id)
		{
			var userservice = _context.UserServices.Find(id);
			if (userservice == null || userservice.Image == null)
			{
				return NotFound();
			}

			return File(userservice.Image, "image/jpeg");
		}






		[Route("Service/{id:int}")]
		public async Task<IActionResult> UserService(int id)
		{
			var service = new UserService();
			using (var client = new HttpClient())
			{
				var token = Request.Cookies["token"];

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);

				using (var responce = await client.GetAsync($"http://localhost:5292/api/UserService/Service/" + id))
				{
					var result = await responce.Content.ReadAsStringAsync();
					service = JsonConvert.DeserializeObject<UserService>(result);
				}

			}



			 //await _context.UserServices.Include(s => s.Service).Include(u => u.User).Include(r => r.Reviews).ThenInclude(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
			return View(service);
		}








		[HttpPost]
		public async Task<IActionResult> AddReview(String Comment, int UserServiceId)
		{

			if (ModelState.IsValid)
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userId == null)
				{
					return RedirectToAction("Register", "Account");
				}

				Review review = new Review
				{
					UserId = userId,
					Comment = Comment,
					UserServiceId = UserServiceId,
					CreatedAt = DateTime.Now.Date
				};
				_context.review.Add(review);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("UserService", "UserService", new { id = UserServiceId });
		}




	}
}
