using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibePlace.Data.Models
{
	public class Places
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Название обязательно")]
		[StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Адрес обязателен")]
		[StringLength(200, ErrorMessage = "Адрес не должен превышать 200 символов")]
		[Display(Name = "Адрес")]
		public string Address { get; set; }

		[Url(ErrorMessage = "Некорректная ссылка на карту")]
		[Display(Name = "Ссылка на карту")]
		public string? AddressLink { get; set; }

		[Required(ErrorMessage = "Описание обязательно")]
		[StringLength(50, ErrorMessage = "Описание не должно превышать 1000 символов")]
		[Display(Name = "Описание")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Телефон обязателен")]
		[RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Некорректный формат телефона")]
		[Display(Name = "Телефон")]
		public string PhoneNumber { get; set; }

		[Range(0, 5, ErrorMessage = "Рейтинг должен быть между 0 и 5")]
		public double? Rating { get; set; }

		[Required(ErrorMessage = "Цена обязательна")]
		[Range(0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательной")]
		[Display(Name = "Цена")]
		public double Price { get; set; }


		public Byte[]? Image { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Вместимость должна быть положительной")]
		[Display(Name = "Вместимость")]
		public int? Capacity { get; set; }




		[Required(ErrorMessage = "Категория обязательна")]
		public int CategoryId { get; set; }
		public Category? Category { get; set; }

		public string? UserId { get; set; }
		public AppUser? User { get; set; }

		[Display(Name = "Город")]
		public int? CityId { get;set; }
		public City? City { get; set; }

	
		public ICollection<Review> Reviews { get; set; } = []; 
		public ICollection<PlaceImage> Images { get; set; } = [];
		public ICollection<RatingPlace> Ratings { get; set; } = [];

		public ICollection<ServiceToPlace?> ServiceToPlaces { get; set; } = [];
	}

}
