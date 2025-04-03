using VibePlace.Migrations;

namespace VibePlace.Data.Models
{
	public class Review
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public int? Like { get; set; }

		public int PlaceId { get; set; }
		public Places Place { get; set; }

		public string UserId { get; set; }
		public AppUser User { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
