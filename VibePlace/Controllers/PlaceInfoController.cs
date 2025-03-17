using Microsoft.AspNetCore.Mvc;

namespace VibePlace.Controllers
{
    public class PlaceInfoController : Controller
    {
        public IActionResult placeinfo()
        {
            return View();
        }
    }
}
