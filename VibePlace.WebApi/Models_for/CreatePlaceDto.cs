namespace VibePlace.WebApi.Models_for
{
	public class CreatePlaceDto
	{
		public string PlaceJson { get; set; }

		public IFormFile Image { get; set; }

		public List<IFormFile> Photos { get; set; }

		public List<int> SelectedServices { get; set; }
	}
}
