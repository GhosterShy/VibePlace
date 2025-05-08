namespace VibePlace.WebApi.Models
{
	public class Review
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public int Like { get; set; } = 0;

		public int? PlaceId { get; set; }
		public Places? Place { get; set; }

		public int? UserServiceId { get; set; }
		public UserService? UserService { get; set; }

		public string UserId { get; set; }
		public AppUser User { get; set; }

		public ICollection<ReviewLike?> ReviewLikes { get; set; } = [];

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
