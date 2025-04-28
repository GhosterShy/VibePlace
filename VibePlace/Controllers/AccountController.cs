using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VibePlace.Data;
using VibePlace.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VibePlace.Controllers
{
    public class AccountController : Controller
    {

		private UserManager<AppUser> _accountManager;
		private SignInManager<AppUser> _singInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly RoleManager<IdentityRole> _roleManager;


		public AccountController(UserManager<AppUser> accountManager, SignInManager<AppUser> singInManager, UserManager<AppUser> userManager,  ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
		{
			_accountManager = accountManager;
			_singInManager = singInManager;
			_userManager = userManager;
			_logger = logger;
			_roleManager = roleManager;
		}



		public IActionResult Index()
		{
			return View();
		}




		[HttpPost]
		public async Task<IActionResult> Index(AccountModel account)
		{
			AppUser appUser = await _accountManager.FindByEmailAsync(account.Login);
			if (appUser != null)
			{
				var result = await _singInManager.PasswordSignInAsync(appUser, account.Password, false, false);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
			}

			return View(account);


		}


		public IActionResult Register()
		{
			
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				
				var user = new AppUser { UserName = model.Name, Email = model.Email};
				var result = await _accountManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, model.SelectedRole);
					await _singInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View(model);
		}


	
		/// <Google>
		public async Task Login()
		{
			await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
				new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback") });
		}






		//[HttpGet]
		//public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
		//{
		//	if (remoteError != null)
		//	{
		//		_logger.LogWarning("Google remote error: {RemoteError}", remoteError);
		//		return RedirectToAction("Login");
		//	}

		//	var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		//	if (!result.Succeeded || result.Principal == null)
		//	{
		//		_logger.LogWarning("Google login failed. Failure: {Error}, Succeeded: {Succeeded}", result.Failure?.Message, result.Succeeded);
		//		return RedirectToAction("Login");
		//	}

		//	var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
		//	var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

		//	if (email == null)
		//	{
		//		_logger.LogWarning("Email claim is missing.");
		//		return RedirectToAction("Login");
		//	}

		//	// Здесь можно сохранить пользователя в базе данных или найти
		//	var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
		//	claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, name ?? "User"));
		//	claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, email));

		//	await HttpContext.SignInAsync(
		//		CookieAuthenticationDefaults.AuthenticationScheme,
		//		new ClaimsPrincipal(claimsIdentity));

		//	return RedirectToAction("Index", "Home");
		//}














		public async Task<IActionResult> ExternalLoginCallback()
		{
			var authenticateResult = await HttpContext.AuthenticateAsync(
				GoogleDefaults.AuthenticationScheme);

			if (!authenticateResult.Succeeded)
			{
				_logger.LogWarning("Google login failed. Failure: {Failure}, Succeeded: {Succeeded}",
					authenticateResult.Failure?.ToString(), authenticateResult.Succeeded);
				return RedirectToAction("Login");
			}


			var claims = authenticateResult.Principal.Identities
				.FirstOrDefault()?.Claims;

			var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

			var firstName = name?.Split(' ')[0];

			var user = await _userManager.FindByEmailAsync(email);

			

			if (user == null)
			{
				user = new AppUser
				{
					UserName = firstName,
					Email = email
				};

				var result = await _userManager.CreateAsync(user);
				if (!result.Succeeded)
				{
					return RedirectToAction("Login");
				}

			}


			await _singInManager.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home");
		}














		public IActionResult Logout()
		{
			return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
		}




		/// </Google>









		public async Task<IActionResult> Profile()
		{
			var user = await _userManager.GetUserAsync(User);
			return View(user);
		}

	}
}
