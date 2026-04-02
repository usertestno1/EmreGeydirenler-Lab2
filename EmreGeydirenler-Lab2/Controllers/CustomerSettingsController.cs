using System.Security.Claims;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Allows customers to manage their profile and security preferences.
    [Authorize(Roles = "Customer")]
    public class CustomerSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays and updates company profile information
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var customer = await _context.Customers.FindAsync(customerId.Value);
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
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var customer = await _context.Customers.FindAsync(customerId.Value);
            if (customer is null)
            {
                return NotFound();
            }

            customer.CompanyName = input.CompanyName;
            customer.TaxNumber = input.TaxNumber;
            customer.Address = input.Address;

            _context.AuditTrails.Add(new AuditTrail
            {
                ActionDescription = "Customer updated company profile details.",
                Timestamp = DateTime.UtcNow,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = customerId.Value,
                User = null!
            });

            await _context.SaveChangesAsync();
            TempData["ProfileMessage"] = "Profile information updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        // Manages account security settings like 2FA and email alerts
        [HttpGet]
        public async Task<IActionResult> Security()
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var setting = await _context.AccountSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.CustomerId == customerId.Value);

            if (setting is null)
            {
                setting = new AccountSetting
                {
                    CustomerId = customerId.Value,
                    TwoFactorEnabled = false,
                    ReceiveEmailAlerts = true,
                    Customer = null!
                };
            }

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Security(AccountSetting input, string? newPassword)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId.Value);

            if (customer is null)
            {
                return NotFound();
            }

            var setting = await _context.AccountSettings
                .FirstOrDefaultAsync(a => a.CustomerId == customerId.Value);

            if (setting is null)
            {
                setting = new AccountSetting
                {
                    CustomerId = customerId.Value,
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

            var trimmedNewPassword = newPassword?.Trim();
            var passwordChanged = !string.IsNullOrWhiteSpace(trimmedNewPassword);
            if (passwordChanged)
            {
                customer.Password = trimmedNewPassword!;
            }

            _context.AuditTrails.Add(new AuditTrail
            {
                ActionDescription = "Customer updated security preferences (2FA and email alerts).",
                Timestamp = DateTime.UtcNow,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = customerId.Value,
                User = null!
            });

            if (passwordChanged)
            {
                _context.AuditTrails.Add(new AuditTrail
                {
                    ActionDescription = "Customer changed account password from Security settings.",
                    Timestamp = DateTime.UtcNow,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserId = customerId.Value,
                    User = null!
                });
            }

            await _context.SaveChangesAsync();
            TempData["SecurityMessage"] = passwordChanged
                ? "Security settings updated successfully. The password for your current account was updated."
                : "Security settings updated successfully.";
            return RedirectToAction(nameof(Security));
        }

        private int? GetCurrentCustomerId()
        {
            if (!User.IsInRole("Customer"))
            {
                return null;
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var customerId) ? customerId : null;
        }
    }
}
