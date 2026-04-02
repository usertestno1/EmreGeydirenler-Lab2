using System.Collections.Generic;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Manages customer subscription plans, upgrades, and usage monitoring.
    public class SubscriptionController : Controller
    {
        // Lists all available plans (Basic, Pro, Enterprise)
        public IActionResult Plans()
        {
            // Passing dummy data to the view to simulate a database query for Lab 2
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan
                {
                    Id = 1,
                    PlanName = "Basic",
                    MonthlyPrice = 19.99m,
                    MaxUsers = 5,
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    PlanName = "Pro",
                    MonthlyPrice = 49.99m,
                    MaxUsers = 20,
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    PlanName = "Enterprise",
                    MonthlyPrice = 99.99m,
                    MaxUsers = 100,
                },
            };
            return View(plans);
        }

        // Shows the current active subscription for the logged-in customer
        public IActionResult MySubscription()
        {
            // Dummy data representing the user's active subscription and usage progress
            ViewBag.CurrentPlan = "Pro Plan";
            ViewBag.DaysRemaining = 15;
            ViewBag.StorageUsed = "15.5 GB / 50 GB";

            return View();
        }

        // Handles the upgrade process for a specific plan
        public IActionResult Upgrade(int planId)
        {
            ViewBag.SelectedPlanId = planId;
            return View();
        }
    }
}
