using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibePlace.Data;
using VibePlace.Models;

namespace VibePlace.Controllers
{
    public class AccountController : Controller
    {

		private UserManager<AppUser> _accountManager;
		private SignInManager<AppUser> _singInManager;


		public AccountController(UserManager<AppUser> accountManager, SignInManager<AppUser> singInManager)
		{
			_accountManager = accountManager;
			_singInManager = singInManager;
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
				var user = new AppUser { UserName = model.Name, Email = model.Email };
				var result = await _accountManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
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

	}
}
