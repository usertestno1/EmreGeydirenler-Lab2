using System;
using System.Linq;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Manages customer subscription plans, upgrades, and usage monitoring.
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int DemoCustomerId = 2001;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lists all available plans (Basic, Pro, Enterprise)
        public async Task<IActionResult> Plans()
        {
            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.MonthlyPrice)
                .ToListAsync();

            return View(plans);
        }

        // Shows the current active subscription for the logged-in customer
        public async Task<IActionResult> MySubscription()
        {
            var subscription = await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.CustomerId == DemoCustomerId && s.Status == "Active");

            var latestUsage = await _context.UsageRecords
                .AsNoTracking()
                .Where(u => u.CustomerId == DemoCustomerId)
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
                .FirstOrDefaultAsync(s => s.CustomerId == DemoCustomerId && s.Status == "Active");

            ViewBag.SelectedPlanId = planId;
            ViewBag.CurrentPlanName = currentSubscription?.SubscriptionPlan.PlanName ?? "None";
            ViewBag.CurrentPlanPrice = currentSubscription?.SubscriptionPlan.MonthlyPrice ?? 0m;
            ViewBag.NewPlanName = selectedPlan.PlanName;
            ViewBag.NewPlanPrice = selectedPlan.MonthlyPrice;

            return View();
        }
    }
}
