namespace VibePlace.WebApi.Models
{
	public class Service
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<ServiceToPlace?> ServiceToPlaces { get; set; } = [];


		public ICollection<UserService?> UserServices { get; set; } = [];
	}
}
