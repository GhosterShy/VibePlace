using VibePlace.Data.Models;

namespace VibePlace.Models
{
	public class PlaceToService
	{
		public Places places { get; set; }
		public ICollection<Service?> services { get; set; }
	}
}
