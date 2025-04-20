namespace VibePlace.Data.Models
{
	public class City
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Places>? places { get; set; }
	}
}
