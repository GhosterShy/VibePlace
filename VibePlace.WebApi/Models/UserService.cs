namespace VibePlace.WebApi.Models
{
	public class UserService
	{
		public int Id { get; set; }

		public string? UserId { get; set; }
		public AppUser? User { get; set; }

		public int ServiceId { get; set; }
		public Service? Service { get; set; }

		public Byte[]? Image { get; set; }
		public string Number { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }


		public ICollection<Review>? Reviews { get; set; }



	}
}
