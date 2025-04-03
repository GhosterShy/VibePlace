using VibePlace.Migrations;

namespace VibePlace.Data.Models
{
	public class PlaceImage
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }

		public int PlaceId { get; set; }
		public Places Place { get; set; }
	}
}
