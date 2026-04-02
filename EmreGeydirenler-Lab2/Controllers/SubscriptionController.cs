using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Manages customer subscription plans, upgrades, and usage monitoring.
    [Authorize(Roles = "Customer")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lists all available active plans.
        public async Task<IActionResult> Plans()
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.MonthlyPrice)
                .ToListAsync();

            var currentPlanId = await _context.Subscriptions
                .AsNoTracking()
                .Where(s => s.CustomerId == customerId.Value && s.Status == "Active")
                .Select(s => (int?)s.PlanId)
                .FirstOrDefaultAsync();

            ViewBag.CurrentPlanId = currentPlanId;

            return View(plans);
        }

        // Shows the current active subscription for the logged-in customer
        public async Task<IActionResult> MySubscription()
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var subscription = await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.CustomerId == customerId.Value && s.Status == "Active");

            var latestUsage = await _context.UsageRecords
                .AsNoTracking()
                .Where(u => u.CustomerId == customerId.Value)
                .OrderByDescending(u => u.LogDate)
                .FirstOrDefaultAsync();

            var daysRemaining = subscription is null
                ? 0
                : Math.Max((subscription.EndDate.Date - DateTime.UtcNow.Date).Days, 0);

            var storageLimitGb = subscription?.SubscriptionPlan.PlanName switch
            {
                "Starter" => 5,
                "Growth" => 50,
                "Professional" => 100,
                "Scale" => 250,
                "Enterprise" => 500,
                _ => 50
            };

            var apiLimit = subscription?.SubscriptionPlan.PlanName switch
            {
                "Starter" => 1000,
                "Growth" => 10000,
                "Professional" => 25000,
                "Scale" => 100000,
                _ => 100000
            };

            var usedGb = latestUsage is null ? 0 : Math.Round((double)latestUsage.StorageUsedMb / 1024, 1);
            var usedApi = latestUsage?.ApiRequestsCount ?? 0;
            var storagePercent = storageLimitGb == 0 ? 0 : (int)Math.Min((usedGb / storageLimitGb) * 100, 100);
            var apiPercent = apiLimit == 0 ? 0 : (int)Math.Min((double)usedApi / apiLimit * 100, 100);

            ViewBag.CurrentPlan = subscription?.SubscriptionPlan.PlanName ?? "No Active Plan";
            ViewBag.DaysRemaining = daysRemaining;
            ViewBag.NextBillingDate = subscription?.EndDate.ToString("dd MMM yyyy") ?? "N/A";
            ViewBag.StorageUsed = $"{usedGb:0.0} GB / {storageLimitGb} GB";
            ViewBag.StoragePercent = storagePercent;
            ViewBag.ApiUsed = usedApi;
            ViewBag.ApiLimit = apiLimit;
            ViewBag.ApiPercent = apiPercent;

            return View();
        }

        // Handles the upgrade process for a specific plan
        public async Task<IActionResult> Upgrade(int planId)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var selectedPlan = await _context.SubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == planId && p.IsActive);

            if (selectedPlan is null)
            {
                return NotFound();
            }

            var currentSubscription = await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.CustomerId == customerId.Value && s.Status == "Active");

            if (currentSubscription is null)
            {
                return NotFound();
            }

            ViewBag.SelectedPlanId = planId;
            ViewBag.CurrentPlanName = currentSubscription?.SubscriptionPlan.PlanName ?? "None";
            ViewBag.CurrentPlanPrice = currentSubscription?.SubscriptionPlan.MonthlyPrice ?? 0m;
            ViewBag.NewPlanName = selectedPlan.PlanName;
            ViewBag.NewPlanPrice = selectedPlan.MonthlyPrice;
            ViewBag.PriceDifference = selectedPlan.MonthlyPrice - currentSubscription.SubscriptionPlan.MonthlyPrice;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUpgrade(int planId)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId is null)
            {
                return Forbid();
            }

            var currentSubscription = await _context.Subscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.CustomerId == customerId.Value && s.Status == "Active");

            var targetPlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == planId && p.IsActive);

            if (currentSubscription is null || targetPlan is null)
            {
                return NotFound();
            }

            var priceDifference = targetPlan.MonthlyPrice - currentSubscription.SubscriptionPlan.MonthlyPrice;

            if (priceDifference > 0)
            {
                _context.Invoices.Add(new Invoice
                {
                    IssueDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(14),
                    TotalAmount = priceDifference,
                    IsPaid = false,
                    CustomerId = customerId.Value,
                    Customer = null!
                });
            }

            currentSubscription.PlanId = targetPlan.Id;

            _context.AuditTrails.Add(new AuditTrail
            {
                ActionDescription = $"Customer confirmed subscription upgrade from {currentSubscription.SubscriptionPlan.PlanName} to {targetPlan.PlanName}. Current-period charge difference: AUD {priceDifference:0.00}.",
                Timestamp = DateTime.UtcNow,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = customerId.Value,
                User = null!
            });

            await _context.SaveChangesAsync();

            TempData["UpgradeMessage"] = priceDifference > 0
                ? $"Plan updated. A pending invoice of AUD {priceDifference:0.00} was created for this billing period."
                : "Plan updated successfully. New monthly pricing applies from the next billing cycle.";

            return RedirectToAction(nameof(MySubscription));
        }

        private int? GetCurrentCustomerId()
        {
            // Reads the logged-in customer's id from cookie claims.
            if (!User.IsInRole("Customer"))
            {
                return null;
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var customerId) ? customerId : null;
        }
    }
}
