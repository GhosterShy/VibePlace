using Microsoft.AspNetCore.Identity;

namespace VibePlace.WebApi.Models
{
	public class AppUser : IdentityUser
	{
		public Byte[]? Logo { get; set; }

		public DateTime addDate { get; set; } = DateTime.UtcNow;
		public ICollection<Places> Places { get; set; } = [];
		public ICollection<ReviewLike> ReviewLike { get; set; } = [];

		public ICollection<RatingPlace> Ratings { get; set; } = [];
		public ICollection<UserService?> UserServices { get; set; } = [];
	}
}
