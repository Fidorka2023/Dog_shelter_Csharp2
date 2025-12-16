using Microsoft.AspNetCore.Mvc;

namespace DogShelterMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

