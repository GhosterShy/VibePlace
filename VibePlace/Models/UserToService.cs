using VibePlace.Data.Models;

namespace VibePlace.Models
{
	public class UserToService
	{
		public ICollection<Service> Services { get; set; }
		public ICollection<UserService> UsersServices { get; set; }
	}
}
