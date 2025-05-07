namespace VibePlace.WebApi.Models
{
	public class PlaceImage
	{
		public int Id { get; set; }
		public Byte[]? ImageUrl { get; set; }

		public int PlaceId { get; set; }
		public Places Place { get; set; }
	}
}
