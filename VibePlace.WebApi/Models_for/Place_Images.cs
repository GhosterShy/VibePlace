
using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Models_for
{
	public class Place_Images
	{
		public Places places { get; set; }
		public ICollection<Service?> services { get; set; }
		public ICollection<PlaceImage> placeImages { get; set; }
	}
}
