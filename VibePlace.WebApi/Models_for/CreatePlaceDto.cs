using VibePlace.WebApi.Models;

namespace VibePlace.WebApi.Models_for
{
	public class CreatePlaceDto
	{

		
		public string Name { get; set; }
		public string Address { get; set; }
		public string? AddressLink { get; set; }
		public string Description { get; set; }
		public string PhoneNumber { get; set; }
		public double Price { get; set; }
		public int? Capacity { get; set; }
		public int CategoryId { get; set; }
		public int? CityId { get; set; }


		public IFormFile Image { get; set; }

		public List<IFormFile> Photos { get; set; }

		public ICollection<int> SelectedServices { get; set; } = [];
	}
}
