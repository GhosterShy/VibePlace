using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;
using VibePlace.AppFilter;
using VibePlace.AppMiddleWare;
using VibePlace.Data;
using VibePlace.Models;
using VibePlace.Models.Interfaces;
using VibePlace.Services;

var builder = WebApplication.CreateBuilder(args);



#region Google

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
	options.ClientId = builder.Configuration["Authentication:ClientId"];
	options.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
	options.SignInScheme = IdentityConstants.ExternalScheme;
});

#endregion


builder.Services.AddScoped<IMassage, EmailSender>();
#region DataBase

builder.Services.AddDbContext<AppIdentityDBContext>
	(options => options.UseSqlServer(
		builder.Configuration["ConnectionStrings:DefaultConnection"]));


builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDBContext>()
	.AddRoles<IdentityRole>() 
	.AddDefaultTokenProviders();


#endregion




#region log
Log.Logger = new LoggerConfiguration()
	.WriteTo.Seq("http://localhost:5341")
	.MinimumLevel.Debug()
	.CreateLogger();

builder.Host.UseSerilog();
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

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<PlaceService>();



//Filter
//builder.Services.AddControllers(options =>
//{
//	options.Filters.Add<IEFilter>();
//});






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
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//app.UseMiddleware<IEMiddleware>();


builder.Services.AddLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



#region UserRole

//using (var scope = app.Services.CreateScope())
//{
//	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//	string[] requiredRoles = { "Admin", "Organizator", "User", "Service" }; 

//	foreach (var role in requiredRoles)
//	{
//		if (!await roleManager.RoleExistsAsync(role))
//		{
//			await roleManager.CreateAsync(new IdentityRole(role));
//			Console.WriteLine($"Ðîëü '{role}' ñîçäàíà");
//		}
//	}
//}

#endregion


app.Run();
