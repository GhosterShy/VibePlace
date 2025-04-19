using VibePlace.Data.Models;

namespace VibePlace.Models
{
	public class Place_Images
	{
		public Places places { get; set; }
		public ICollection<Service?> services { get; set; }
		public ICollection<PlaceImage> placeImages { get; set; }
	}
}
