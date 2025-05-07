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
using System.ComponentModel.DataAnnotations;
using VibePlace.Models.Interfaces;

namespace VibePlace.Controllers
{
    public class AccountController : Controller
    {


		private TokenService _tokenService;
		private UserManager<AppUser> _accountManager;
		private SignInManager<AppUser> _singInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMassage _emailSender;


		public AccountController(UserManager<AppUser> accountManager, SignInManager<AppUser> singInManager, UserManager<AppUser> userManager,  ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, TokenService tokenService,IMassage emailSender)
		{
			_accountManager = accountManager;
			_singInManager = singInManager;
			_userManager = userManager;
			_logger = logger;
			_roleManager = roleManager;
			_tokenService = tokenService;
			_emailSender = emailSender;
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
					var token = await _tokenService.GenerateAccessToken(appUser);
					Response.Cookies.Append("token",token);
					if (await _userManager.IsInRoleAsync(appUser, "Organizator"))
					{
						return RedirectToAction("Index", "Organizator");
					}
					return RedirectToAction("Index", "Home");
				}
			}

			return View(account);


		}



		public IActionResult VerifyEmail()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> VerifyEmail(EmailVerificationModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null && user.EmailConfirmed)
			{
				ModelState.AddModelError("Email", "Этот email уже используется.");
				return View(model);
			}

			string code = new Random().Next(100000, 999999).ToString();
			TempData["VerificationCode"] = code;
			TempData["UserEmail"] = model.Email;

			var subject = "Подтверждение Email";
			var body = $"Ваш код подтверждения: {code}";

			_emailSender.SendMessage(model.Email, subject, body);

			return RedirectToAction("ConfirmCode");
		}




		public IActionResult ConfirmCode()
		{
			return View();
		}



		[HttpPost]
		public IActionResult ConfirmCode(string code)
		{
			var expectedCode = TempData["VerificationCode"] as string;
			var email = TempData["UserEmail"] as string;

			if (code == expectedCode)
			{
				TempData["ConfirmedEmail"] = email;
				return RedirectToAction("Register");
			}

			ViewBag.Error = "Неверный код.";
			TempData["VerificationCode"] = expectedCode;
			TempData["UserEmail"] = email;
			return View();
		}





		public IActionResult Register()
		{
			if (TempData["ConfirmedEmail"] == null)
				return RedirectToAction("VerifyEmail");

			TempData.Keep("ConfirmedEmail");

			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var email = TempData["ConfirmedEmail"] as string;
			if (email == null)
				return RedirectToAction("VerifyEmail");


			if (ModelState.IsValid)
			{

				var user = new AppUser { UserName = model.Name, Email = email, EmailConfirmed = true };
				var result = await _accountManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, model.SelectedRole);
					await _singInManager.SignInAsync(user, isPersistent: false);

					if (model.SelectedRole == "Organizator")
					{
						return RedirectToAction("Index", "Organizator");
					}
					else
						return RedirectToAction("Index", "Home");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			TempData.Keep("ConfirmedEmail");
			return View(model);
		}
















		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _singInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
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












		
		//[ValidateAntiForgeryToken]
		//public IActionResult Logout()
		//{
		//	return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
		//}




		/// </Google>









		public async Task<IActionResult> Profile()
		{
			var user = await _userManager.GetUserAsync(User);
			return View(user);
		}

	}
}
