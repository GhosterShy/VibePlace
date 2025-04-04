﻿namespace VibePlace.Data.Models
{
	public class Places
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string? AddressLink { get; set; }
		public string Description { get; set; } 
		public string PhoneNumber { get; set; }
		public double Rating { get; set; }
		public double Price { get; set; }
		public string? Image { get; set; }
		public int? Capacity { get; set; }
		


	

		public int CategoryId { get; set; }
		public Category? Category { get; set; }

		public string? UserId { get; set; }
		public AppUser? User { get; set; }


		public ICollection<Review?> Reviews { get; set; } = []; 
		public ICollection<PlaceImage?> Images { get; set; } = [];


		public ICollection<ServiceToPlace?> ServiceToPlaces { get; set; } = [];
	}

}
