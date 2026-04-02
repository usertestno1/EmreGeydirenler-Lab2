using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Handles customer billing and invoice history.
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves and displays invoice records with related customer and subscription details.
        /// </summary>
        /// <returns>
        /// A view containing invoice data ordered by issue date in descending order.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            var invoiceQuery = _context.Invoices
                .Include(i => i.Customer)
                .ThenInclude(c => c.Subscriptions)
                .ThenInclude(s => s.SubscriptionPlan)
                .AsNoTracking()
                .OrderByDescending(i => i.IssueDate)
                .AsQueryable();

            if (User.IsInRole("Customer"))
            {
                var customerId = GetCurrentCustomerId();
                if (customerId is null)
                {
                    return Forbid();
                }

                invoiceQuery = invoiceQuery.Where(i => i.CustomerId == customerId.Value);
            }

            var invoices = await invoiceQuery.ToListAsync();

            return View(invoices);
        }

        /// <summary>
        /// Displays a detail page for a specific invoice.
        /// </summary>
        /// <param name="id">The unique identifier of the invoice.</param>
        /// <returns>A view with invoice detail information.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var invoiceQuery = _context.Invoices
                .Include(i => i.Customer)
                .ThenInclude(c => c.Subscriptions)
                .ThenInclude(s => s.SubscriptionPlan)
                .AsNoTracking()
                .Where(i => i.Id == id)
                .AsQueryable();

            if (User.IsInRole("Customer"))
            {
                var customerId = GetCurrentCustomerId();
                if (customerId is null)
                {
                    return Forbid();
                }

                invoiceQuery = invoiceQuery.Where(i => i.CustomerId == customerId.Value);
            }

            var invoice = await invoiceQuery.FirstOrDefaultAsync();

            if (invoice is null)
            {
                return NotFound();
            }

            var subscription = invoice.Customer.Subscriptions?
                .Where(s => s.Status == "Active")
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault();

            ViewBag.SubscriptionPlanName = subscription?.SubscriptionPlan.PlanName ?? "N/A";
            return View(invoice);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id && i.CustomerId == customerId.Value);

            if (invoice is null)
            {
                return NotFound();
            }

            if (!invoice.IsPaid)
            {
                invoice.IsPaid = true;

                _context.AuditTrails.Add(new AuditTrail
                {
                    ActionDescription = $"Customer completed invoice payment for invoice #{invoice.Id} (Amount: AUD {invoice.TotalAmount:0.00}).",
                    Timestamp = DateTime.UtcNow,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserId = customerId.Value,
                    User = null!
                });

                await _context.SaveChangesAsync();
                TempData["InvoiceMessage"] = $"Invoice #{invoice.Id} marked as paid.";
            }

            return RedirectToAction(nameof(Index));
        }

        private int? GetCurrentCustomerId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var customerId) ? customerId : null;
        }
    }
}
