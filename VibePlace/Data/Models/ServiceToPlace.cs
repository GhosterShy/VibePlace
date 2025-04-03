namespace VibePlace.Data.Models
{
	public class ServiceToPlace
	{
	
		public int PlaceId { get; set; }
		public Places Place { get; set; }

		public int ServisId { get; set; }
		public Service Service { get; set; }


	}
}
