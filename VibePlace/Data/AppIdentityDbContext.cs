using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VibePlace.Data.Models;
using VibePlace.Services;

namespace VibePlace.Data
{
	public class AppIdentityDBContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) : base(options){}

		public DbSet<Places> places { get; set; }
		public DbSet<Category> categories { get; set; }
		public DbSet<PlaceImage> placeimage { get; set; }
		public DbSet<Review> review { get; set; }
		public DbSet<ReviewLike> reviewLike { get; set; }
		public DbSet<Service> services { get; set; }
		public DbSet<ServiceToPlace> serviceToPlace { get; set; }
		public DbSet<City> cities { get; set; }
		



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>()
				.HasMany(c => c.places)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

			modelBuilder.Entity<City>()
				.HasMany(c => c.places)
				.WithOne(p => p.City)
				.HasForeignKey(p => p.CityId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Places>()
				.HasMany(c => c.Reviews)
				.WithOne(p => p.Place)
				.HasForeignKey(p => p.PlaceId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Places>()
				.HasMany(c => c.Images)
				.WithOne(p => p.Place)
				.HasForeignKey(p => p.PlaceId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<ReviewLike>()
				.HasKey(ps => new { ps.UserId, ps.ReviewId});

			modelBuilder.Entity<ReviewLike>()
				.HasOne(c => c.User)
				.WithMany(p => p.ReviewLike)
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<ReviewLike>()
				.HasOne(c => c.Review)
				.WithMany(p => p.ReviewLikes)
				.HasForeignKey(p => p.ReviewId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<ServiceToPlace>()
				.HasKey(ps => new { ps.PlaceId, ps.ServisId });

			modelBuilder.Entity<ServiceToPlace>()
				.HasOne(c => c.Place)
				.WithMany(p => p.ServiceToPlaces)
				.HasForeignKey(p => p.PlaceId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ServiceToPlace>()
				.HasOne(c => c.Service)
				.WithMany(p => p.ServiceToPlaces)
				.HasForeignKey(p => p.ServisId)
				.OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(modelBuilder);
		}
	}
}
