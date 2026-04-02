using System;
using System.Collections.Generic;
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

        // Shows high-level platform statistics (MRR, Total Users)
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
            ViewBag.MonthlyRevenue = totalMonthlyRevenue.ToString("C");
            ViewBag.ActiveSubscriptions = activeSubscriptions;

            return View();
        }

        // Displays system audit logs for accountability and security monitoring
        public async Task<IActionResult> AuditLogs()
        {
            var logs = await _context.AuditTrails
                .Include(a => a.User)
                .AsNoTracking()
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return View(logs);
        }

        // Allows admin to configure subscription plans and modules
        public async Task<IActionResult> ManagePlans()
        {
            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .OrderBy(p => p.MonthlyPrice)
                .ToListAsync();

            return View(plans);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
