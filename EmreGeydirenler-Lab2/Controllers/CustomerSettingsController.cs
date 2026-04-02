using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Allows customers to manage their profile and security preferences.
    public class CustomerSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int DemoCustomerId = 2001;

        public CustomerSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays and updates company profile information
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var customer = await _context.Customers.FindAsync(DemoCustomerId);
            if (customer is null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Customer input)
        {
            var customer = await _context.Customers.FindAsync(DemoCustomerId);
            if (customer is null)
            {
                return NotFound();
            }

            customer.CompanyName = input.CompanyName;
            customer.TaxNumber = input.TaxNumber;
            customer.Address = input.Address;

            await _context.SaveChangesAsync();
            TempData["ProfileMessage"] = "Profile information updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        // Manages account security settings like 2FA and email alerts
        [HttpGet]
        public async Task<IActionResult> Security()
        {
            var setting = await _context.AccountSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.CustomerId == DemoCustomerId);

            if (setting is null)
            {
                setting = new AccountSetting
                {
                    CustomerId = DemoCustomerId,
                    TwoFactorEnabled = false,
                    ReceiveEmailAlerts = true,
                    Customer = null!
                };
            }

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Security(AccountSetting input)
        {
            var setting = await _context.AccountSettings
                .FirstOrDefaultAsync(a => a.CustomerId == DemoCustomerId);

            if (setting is null)
            {
                setting = new AccountSetting
                {
                    CustomerId = DemoCustomerId,
                    TwoFactorEnabled = input.TwoFactorEnabled,
                    ReceiveEmailAlerts = input.ReceiveEmailAlerts,
                    Customer = null!
                };
                _context.AccountSettings.Add(setting);
            }
            else
            {
                setting.TwoFactorEnabled = input.TwoFactorEnabled;
                setting.ReceiveEmailAlerts = input.ReceiveEmailAlerts;
            }

            await _context.SaveChangesAsync();
            TempData["SecurityMessage"] = "Security settings updated successfully.";
            return RedirectToAction(nameof(Security));
        }
    }
}
