using System;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Secure backend controller for QuTech staff to monitor the platform.
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays high-level administrative metrics for the dashboard.
        /// </summary>
        /// <returns>
        /// A view containing total customer count, active subscription count, and monthly revenue.
        /// </returns>
        public async Task<IActionResult> Dashboard()
        {
            var totalCustomers = await _context.Customers.CountAsync();

            var activeSubscriptions = await _context.Subscriptions
                .CountAsync(s => s.Status == "Active" && s.EndDate >= DateTime.UtcNow);

            var totalMonthlyRevenue = await _context.Subscriptions
                .Where(s => s.Status == "Active" && s.EndDate >= DateTime.UtcNow)
                .Join(
                    _context.SubscriptionPlans,
                    subscription => subscription.PlanId,
                    plan => plan.Id,
                    (subscription, plan) => plan.MonthlyPrice)
                .SumAsync();

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.MonthlyRevenue = $"${totalMonthlyRevenue:0.00}";
            ViewBag.ActiveSubscriptions = activeSubscriptions;

            return View();
        }

        /// <summary>
        /// Retrieves and displays audit trail records for accountability and monitoring.
        /// </summary>
        /// <returns>
        /// A view populated with audit logs ordered by newest timestamp first.
        /// </returns>
        public async Task<IActionResult> AuditLogs()
        {
            var logs = await _context.AuditTrails
                .Include(a => a.User)
                .AsNoTracking()
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return View(logs);
        }

        /// <summary>
        /// Lists subscription plans for administrative management.
        /// </summary>
        /// <returns>
        /// A view containing all subscription plans ordered by monthly price.
        /// </returns>
        public async Task<IActionResult> ManagePlans()
        {
            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .OrderBy(p => p.MonthlyPrice)
                .ToListAsync();

            return View(plans);
        }

        [HttpGet]
        public async Task<IActionResult> ManageCustomers()
        {
            var now = DateTime.UtcNow;
            var customers = await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Select(c => new AdminCustomerListItemViewModel
                {
                    CustomerId = c.Id,
                    FullName = c.FirstName + " " + c.LastName,
                    CompanyName = c.CompanyName,
                    CurrentActivePlan = _context.Subscriptions
                        .Where(s => s.CustomerId == c.Id && s.Status == "Active" && s.EndDate >= now)
                        .OrderByDescending(s => s.EndDate)
                        .Select(s => s.SubscriptionPlan.PlanName)
                        .FirstOrDefault() ?? "No Active Plan",
                    HasUnpaidInvoices = _context.Invoices.Any(i => i.CustomerId == c.Id && !i.IsPaid)
                })
                .ToListAsync();

            return View(customers);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View(new AdminCreateCustomerViewModel
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                CompanyName = string.Empty,
                TaxNumber = string.Empty,
                Address = string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(AdminCreateCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var trimmedEmail = model.Email.Trim();
            var normalizedEmail = trimmedEmail.ToUpper();

            var emailExists = await _context.Customers.AnyAsync(c => c.Email.ToUpper() == normalizedEmail);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "A customer with this email already exists.");
                return View(model);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = trimmedEmail,
                Password = model.Password,
                CompanyName = model.CompanyName.Trim(),
                TaxNumber = model.TaxNumber.Trim(),
                Address = model.Address.Trim(),
                PhoneNumber = "+61 0000 0000",
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            await LogAdminAction($"Created customer account: {customer.FirstName} {customer.LastName} (Id: {customer.Id}).");

            TempData["CustomerMessage"] = "Customer created successfully.";
            return RedirectToAction(nameof(ManageCustomers));
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer is null)
            {
                return NotFound();
            }

            var activePlan = await _context.Subscriptions
                .Where(s => s.CustomerId == id && s.Status == "Active" && s.EndDate >= DateTime.UtcNow)
                .OrderByDescending(s => s.EndDate)
                .Select(s => s.SubscriptionPlan.PlanName)
                .FirstOrDefaultAsync();

            var invoices = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.CustomerId == id)
                .OrderByDescending(i => i.IssueDate)
                .Select(i => new AdminCustomerInvoiceViewModel
                {
                    Id = i.Id,
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate,
                    TotalAmount = i.TotalAmount,
                    IsPaid = i.IsPaid
                })
                .ToListAsync();

            var model = new AdminCustomerEditViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                CompanyName = customer.CompanyName,
                TaxNumber = customer.TaxNumber,
                Address = customer.Address
            };

            ViewBag.CurrentActivePlan = activePlan ?? "No Active Plan";
            ViewBag.Invoices = invoices;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, AdminCustomerEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer is null)
            {
                return NotFound();
            }

            var trimmedEmail = model.Email.Trim();
            var normalizedEmail = trimmedEmail.ToUpper();

            var emailExists = await _context.Customers
                .AnyAsync(c => c.Id != id && c.Email.ToUpper() == normalizedEmail);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "A customer with this email already exists.");
            }

            if (!ModelState.IsValid)
            {
                var invoices = await _context.Invoices
                    .AsNoTracking()
                    .Where(i => i.CustomerId == id)
                    .OrderByDescending(i => i.IssueDate)
                    .Select(i => new AdminCustomerInvoiceViewModel
                    {
                        Id = i.Id,
                        IssueDate = i.IssueDate,
                        DueDate = i.DueDate,
                        TotalAmount = i.TotalAmount,
                        IsPaid = i.IsPaid
                    })
                    .ToListAsync();

                var activePlan = await _context.Subscriptions
                    .Where(s => s.CustomerId == id && s.Status == "Active" && s.EndDate >= DateTime.UtcNow)
                    .OrderByDescending(s => s.EndDate)
                    .Select(s => s.SubscriptionPlan.PlanName)
                    .FirstOrDefaultAsync() ?? "No Active Plan";

                ViewBag.CurrentActivePlan = activePlan;
                ViewBag.Invoices = invoices;

                return View(model);
            }
            customer.FirstName = model.FirstName.Trim();
            customer.LastName = model.LastName.Trim();
            customer.Email = trimmedEmail;
            customer.CompanyName = model.CompanyName.Trim();
            customer.TaxNumber = model.TaxNumber.Trim();
            customer.Address = model.Address.Trim();

            var affectedRows = await _context.SaveChangesAsync();
            if (affectedRows > 0)
            {
                await LogAdminAction($"Updated customer profile: {customer.FirstName} {customer.LastName} (Id: {customer.Id}).");
            }

            TempData["CustomerMessage"] = "Customer details updated successfully.";
            return RedirectToAction(nameof(EditCustomer), new { id = customer.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, int customerId, bool isPaid)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id && i.CustomerId == customerId);

            if (invoice is null)
            {
                return NotFound();
            }

            if (invoice.IsPaid != isPaid)
            {
                invoice.IsPaid = isPaid;
                await _context.SaveChangesAsync();
                await LogAdminAction($"Changed invoice #{invoice.Id} for customer #{invoice.CustomerId} to {(isPaid ? "Paid" : "Unpaid")}." );
            }

            TempData["CustomerMessage"] = $"Invoice #{invoice.Id} marked as {(isPaid ? "Paid" : "Unpaid")}.";
            return RedirectToAction(nameof(EditCustomer), new { id = customerId });
        }

        [HttpGet]
        public async Task<IActionResult> AdminUsers()
        {
            var admins = await _context.Admins
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Select(a => new AdminUserListItemViewModel
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    Email = a.Email
                })
                .ToListAsync();

            return View(admins);
        }

        [HttpGet]
        /// <summary>
        /// Displays the form for creating a new subscription plan.
        /// </summary>
        /// <returns>A view for entering subscription plan details.</returns>
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Creates a new subscription plan when the submitted model is valid.
        /// </summary>
        /// <param name="plan">The subscription plan data submitted from the form.</param>
        /// <returns>
        /// Redirects to the plan list on success, or returns the same view with validation errors.
        /// </returns>
        public async Task<IActionResult> Create(SubscriptionPlan plan)
        {
            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            try
            {
                _context.SubscriptionPlans.Add(plan);
                await _context.SaveChangesAsync();
                await LogAdminAction($"Created subscription plan: {plan.PlanName} (Id: {plan.Id})");
                return RedirectToAction(nameof(ManagePlans));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to create the subscription plan. Please try again.");
                return View(plan);
            }
        }

        [HttpGet]
        /// <summary>
        /// Displays the edit form for an existing subscription plan.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription plan.</param>
        /// <returns>
        /// A view with plan data when found; otherwise <c>NotFound</c>.
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var plan = await _context.SubscriptionPlans.FindAsync(id.Value);
            if (plan is null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Updates an existing subscription plan when the submitted model is valid.
        /// </summary>
        /// <param name="id">The route identifier of the subscription plan.</param>
        /// <param name="plan">The updated subscription plan data from the form.</param>
        /// <returns>
        /// Redirects to the plan list on success, or returns the edit view when validation or update fails.
        /// </returns>
        public async Task<IActionResult> Edit(int id, SubscriptionPlan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            try
            {
                _context.Update(plan);
                await _context.SaveChangesAsync();
                await LogAdminAction($"Updated subscription plan: {plan.PlanName} (Id: {plan.Id})");
                return RedirectToAction(nameof(ManagePlans));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubscriptionPlanExists(plan.Id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(string.Empty, "The plan was modified by another process. Please reload and try again.");
                return View(plan);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to update the subscription plan. Please try again.");
                return View(plan);
            }
        }

        [HttpGet]
        /// <summary>
        /// Displays a delete confirmation page for a subscription plan.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription plan.</param>
        /// <returns>
        /// A confirmation view when found; otherwise <c>NotFound</c>.
        /// </returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var plan = await _context.SubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id.Value);

            if (plan is null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Deletes a subscription plan after confirmation.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription plan to delete.</param>
        /// <returns>
        /// Redirects to the plan list on success, or returns the delete view if the operation fails.
        /// </returns>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan is null)
            {
                return NotFound();
            }

            try
            {
                _context.SubscriptionPlans.Remove(plan);
                await _context.SaveChangesAsync();
                await LogAdminAction($"Deleted subscription plan: {plan.PlanName} (Id: {plan.Id})");
                return RedirectToAction(nameof(ManagePlans));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to delete the subscription plan. It may be used by existing subscriptions.");
                return View(plan);
            }
        }

        private Task<bool> SubscriptionPlanExists(int id)
        {
            return _context.SubscriptionPlans.AnyAsync(e => e.Id == id);
        }

        private async Task LogAdminAction(string actionDescription)
        {
            // Uses the authenticated admin claim to link each admin action to audit trails.
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var adminId))
            {
                return;
            }

            var auditTrail = new AuditTrail
            {
                ActionDescription = actionDescription,
                Timestamp = DateTime.UtcNow,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = adminId,
                User = null!
            };

            _context.AuditTrails.Add(auditTrail);
            await _context.SaveChangesAsync();
        }
    }
}
