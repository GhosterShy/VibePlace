using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Models_for
{
	public class PlaceToService
	{
		public Places places { get; set; }
		public ICollection<Service?> services { get; set; }
	}
}
