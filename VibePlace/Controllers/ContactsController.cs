using Microsoft.AspNetCore.Mvc;

namespace VibePlace.Controllers
{
    public class ContactsController : Controller
    {
        [Route("Contacts")]
        public IActionResult Contacts()
        {
            return View();
        }
    }
}
