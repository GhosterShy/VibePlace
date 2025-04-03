namespace VibePlace.Data.Models
{
	public class Service
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<ServiceToPlace?> ServiceToPlaces { get; set; } = [];

	}
}
