using Microsoft.AspNetCore.Mvc;

namespace VibePlace.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Contacts()
        {
            return View();
        }
    }
}
