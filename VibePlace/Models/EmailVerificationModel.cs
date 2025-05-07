using System.ComponentModel.DataAnnotations;

namespace VibePlace.Models
{
	public class EmailVerificationModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
