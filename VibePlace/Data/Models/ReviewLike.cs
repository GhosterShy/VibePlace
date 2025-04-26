namespace VibePlace.Data.Models
{
	public class ReviewLike
	{
		
		public int Id { get; set; }

		public int ReviewId { get; set; }
		public Review? Review { get; set; }

		public string UserId { get; set; }
		public AppUser? User { get; set; }
	
	
	}
}
