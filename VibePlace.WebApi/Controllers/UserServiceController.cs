using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserServiceController : ControllerBase
	{
		private readonly AppIdentityDBContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;


		public UserServiceController(AppIdentityDBContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}






		[HttpGet]
		[Route("userService")]
		public async Task<ActionResult<UserService>> Get()
		{
			var userServices = await _context.userServices
				.Include(us => us.User)   
				.Include(us => us.Service)  
				.ToListAsync();
			return Ok(userServices);
		}
	}
}
