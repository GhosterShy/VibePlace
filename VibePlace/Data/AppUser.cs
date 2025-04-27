using Microsoft.AspNetCore.Identity;
using VibePlace.Data.Models;

namespace VibePlace.Data
{
	public class AppUser : IdentityUser
	{
		public string? UserImage { get; set;}

		public DateTime addDate { get; set; } = DateTime.UtcNow;
		public ICollection<Places> Places { get; set; } = [];
		public ICollection<ReviewLike> ReviewLike { get; set; } = [];
	}
}
