using VibePlace.Data.Models;

namespace VibePlace.Models
{
	public class PlacesCategoryModel
	{
		public List<Places> Places { get; set; }
		public List<Category> Categories { get; set; }
		public List<Review> Reviews { get; set; }



		public Review NewReview { get; set; } = new Review();
	}
}
