using Microsoft.AspNetCore.Mvc;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Allows customers to manage their profile and security preferences.
    public class CustomerSettingsController : Controller
    {
        // Displays and updates company profile information
        public IActionResult Profile()
        {
            // Dummy data for the profile form to show business-relevant information
            ViewBag.CompanyName = "My Awesome Startup";
            ViewBag.TaxNumber = "TX-987654321";
            return View();
        }

        // Manages account security settings like 2FA and email alerts
        public IActionResult Security()
        {
            // Simulating the AccountSetting model data
            ViewBag.Is2FAEnabled = true;
            ViewBag.ReceiveAlerts = false;
            return View();
        }
    }
}
