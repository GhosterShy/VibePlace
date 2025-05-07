using System.ComponentModel.DataAnnotations;

namespace VibePlace.WebApi.Models
{
	public class RatingPlace
	{
		public int Id { get; set; }
		[Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5.")]
		public int Rating { get; set; } 
		public Places place {  get; set; }
		public int PlaceId { get; set; }
		public AppUser user { get; set; }
		public string UserId {  get; set; }
	}
}
