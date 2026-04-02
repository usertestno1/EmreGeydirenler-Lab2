using System.Security.Claims;
using System.Linq;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        /// <summary>
        /// Displays the shared login page for administrators and customers.
        /// </summary>
        /// <returns>
        /// The login view for anonymous users, or a redirect to the dashboard when already authenticated.
        /// </returns>
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return User.IsInRole("Admin")
                    ? RedirectToAction("Dashboard", "Admin")
                    : RedirectToAction("MySubscription", "Subscription");
            }

            return View(new LoginViewModel { Email = string.Empty, Password = string.Empty });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Authenticates an admin or customer and issues an authentication cookie.
        /// </summary>
        /// <param name="model">The login form data containing email and password.</param>
        /// <returns>
        /// Redirects to role-specific landing page when authentication succeeds; otherwise returns the login view with errors.
        /// </returns>
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var allowedKeys = new[] { nameof(LoginViewModel.Email), nameof(LoginViewModel.Password) };
            var keysToRemove = ModelState.Keys
                .Where(k => !string.IsNullOrEmpty(k) && !allowedKeys.Contains(k))
                .ToList();

            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();
            var password = model.Password.Trim();
            var normalizedEmail = email.ToUpper();

            var admin = await _context.Set<Admin>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email.ToUpper() == normalizedEmail && a.Password == password);

            if (admin is not null)
            {
                var adminClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new(ClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new(ClaimTypes.Email, admin.Email),
                    new(ClaimTypes.Role, "Admin")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrincipal = new ClaimsPrincipal(adminIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    adminPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });

                await LogSuccessfulLoginAsync(admin.Id, "Admin", admin.Email);
                return RedirectToAction("Dashboard", "Admin");
            }

            var customer = await _context.Set<Customer>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email.ToUpper() == normalizedEmail && c.Password == password);

            if (customer is not null)
            {
                var customerClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                    new(ClaimTypes.Name, $"{customer.FirstName} {customer.LastName}"),
                    new(ClaimTypes.Email, customer.Email),
                    new(ClaimTypes.Role, "Customer")
                };

                var customerIdentity = new ClaimsIdentity(customerClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var customerPrincipal = new ClaimsPrincipal(customerIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    customerPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });

                await LogSuccessfulLoginAsync(customer.Id, "Customer", customer.Email);
                return RedirectToAction("MySubscription", "Subscription");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Signs out the current user and clears the authentication cookie.
        /// </summary>
        /// <returns>A redirect to the login page.</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task LogSuccessfulLoginAsync(int userId, string role, string email)
        {
            _context.AuditTrails.Add(new AuditTrail
            {
                ActionDescription = $"Successful {role} login: {email}",
                Timestamp = DateTime.UtcNow,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = userId,
                User = null!
            });

            await _context.SaveChangesAsync();
        }
    }
}