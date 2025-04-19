using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VibePlace.Data;
using VibePlace.Services;

var builder = WebApplication.CreateBuilder(args);


#region Google


#endregion



#region DataBase

builder.Services.AddDbContext<AppIdentityDBContext>
	(options => options.UseSqlServer(
		builder.Configuration["ConnectionStrings:DefaultConnection"]));


builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDBContext>()
	.AddDefaultTokenProviders();


#endregion





builder.Services.Configure<IdentityOptions>(options =>
{
	
	options.User.AllowedUserNameCharacters =
		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ àáâãäå¸æçèéêëìíîïðñòóôõö÷øùüûúýþÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞß";

	options.User.RequireUniqueEmail = true;
});




// Add services to the container.
builder.Services.AddControllersWithViews().AddViewLocalization();



#region Localization

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCulture = new[]
	{
		new CultureInfo("kk-KZ"),
		new CultureInfo("ru-RU"),
		new CultureInfo("en-US")
	};

	options.DefaultRequestCulture = new RequestCulture(culture: "kk-KZ", uiCulture: "kk-KZ");
	options.SupportedCultures = supportedCulture;
	options.SupportedUICultures = supportedCulture;
});



#endregion


builder.Services.AddScoped<PlaceService>();


var app = builder.Build();



#region Localizer
var supportedCulture = new[] { "kk-KZ", "ru-RU", "en-US" };
var localizerOptions = new RequestLocalizationOptions()
	.SetDefaultCulture("kk-KZ")
	.AddSupportedCultures(supportedCulture)
	.AddSupportedUICultures(supportedCulture);

app.UseRequestLocalization(localizerOptions);
#endregion




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");






app.Run();
