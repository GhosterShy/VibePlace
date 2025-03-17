using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibePlace.Data;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AppIdentityDBContext>
	(options => options.UseSqlServer(
		builder.Configuration["ConnectionStrings:DefaultConnection"]));


builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDBContext>()
	.AddDefaultTokenProviders();



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    

app.Run();
