using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

		[HttpGet]
		[Route("Service/{id:int}")]
		public async Task<ActionResult> UserServiceInfo(int id)
		{
			var service = await _context.userServices.Include(s => s.Service).Include(u => u.User).Include(r => r.Reviews).ThenInclude(u => u.User).AsSplitQuery().FirstOrDefaultAsync(i => i.Id == id);
			return Ok(service);
		}




		[HttpGet]
		[Route("FilterService/{id:int}")]
		public async Task<ActionResult> FilterService(int id)
		{
			var services = await _context.userServices
				.Where(s  => s.ServiceId == id)
				.Include(s => s.User)
				.ToListAsync();
			return Ok(services);
		}
	}
}
