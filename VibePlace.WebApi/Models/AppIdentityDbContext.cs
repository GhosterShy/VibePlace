using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VibePlace.WebApi.Models
{
	public class AppIdentityDBContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) : base(options) { }

		public DbSet<Places> places { get; set; }
		public DbSet<Category> categories { get; set; }
		public DbSet<PlaceImage> placeimage { get; set; }
		public DbSet<Review> review { get; set; }
		public DbSet<Service> services { get; set; }
		public DbSet<ServiceToPlace> serviceToPlace { get; set; }
		public DbSet<City> cities { get; set; }
		public DbSet<UserService> userServices { get; set; }
		public DbSet<RatingPlace> ratings { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{



			modelBuilder.Entity<UserService>()
			   .HasOne(us => us.User)
			   .WithMany(u => u.UserServices)
			   .HasForeignKey(us => us.UserId);

			modelBuilder.Entity<UserService>()
				.HasOne(us => us.Service)
				.WithMany(s => s.UserServices)
				.HasForeignKey(us => us.ServiceId);



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

			//для Рейтинга
			modelBuilder.Entity<RatingPlace>()
			   .HasOne(r => r.place)
			   .WithMany(p => p.Ratings)
			   .HasForeignKey(r => r.PlaceId);


			modelBuilder.Entity<RatingPlace>()
			   .HasOne(r => r.user)
			   .WithMany(u => u.Ratings)
			   .HasForeignKey(r => r.UserId)
			   .OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(modelBuilder);
		}
	}
}
