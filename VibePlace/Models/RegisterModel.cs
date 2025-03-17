using System.ComponentModel.DataAnnotations;

namespace VibePlace.Models
{
	public class RegisterModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают.")]
		public string ConfirmPassword { get; set; }
	}
}
