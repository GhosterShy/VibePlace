using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VibePlace.Data.Models;

namespace VibePlace.Data
{
	public class AppIdentityDBContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) : base(options){}

		public DbSet<Places> places { get; set; }
		public DbSet<Category> categories { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>()
				.HasMany(c => c.places)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

			base.OnModelCreating(modelBuilder);
		}
	}
}
