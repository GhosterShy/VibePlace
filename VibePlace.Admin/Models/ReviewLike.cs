namespace VibePlace.Admin.Models
{
	public class ReviewLike
	{
		public int ReviewId { get; set; }
		public Review Review { get; set; }

		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}
