using Microsoft.EntityFrameworkCore;
using VibePlace.Admin.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppIdentityDbContext>
	(options => options.UseSqlServer(
		builder.Configuration["ConnectionStrings:DefaultConnection"]));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Place}/{action=Index}/{id?}");

app.Run();
