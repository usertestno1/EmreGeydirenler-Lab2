using Microsoft.AspNetCore.Mvc;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Controller responsible for the public-facing pages of the QuTech SAAS platform.
    public class HomeController : Controller
    {
        // Displays the main landing page for guests and users
        public IActionResult Index()
        {
            return View();
        }

        // Displays the privacy policy and terms of service
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
