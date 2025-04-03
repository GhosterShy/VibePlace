using Microsoft.AspNetCore.Identity;

namespace VibePlace.Data
{
	public class AppUser : IdentityUser
	{
		public string? UserImage { get; set;}

		public DateTime addDate { get; set; } = DateTime.UtcNow;
	}
}
