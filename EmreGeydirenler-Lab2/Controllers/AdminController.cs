using System;
using System.Collections.Generic;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Secure backend controller for QuTech staff to monitor the platform.
    public class AdminController : Controller
    {
        // Shows high-level platform statistics (MRR, Total Users)
        public IActionResult Dashboard()
        {
            // Dummy analytics for the dashboard view
            ViewBag.TotalCustomers = 1250;
            ViewBag.MonthlyRevenue = "$45,000";
            ViewBag.ActiveSubscriptions = 1100;
            return View();
        }

        // Displays system audit logs for accountability and security monitoring
        public IActionResult AuditLogs()
        {
            // Dummy audit trail records
            var logs = new List<AuditTrail>
            {
                new AuditTrail
                {
                    Id = 1,
                    ActionDescription = "User logged in",
                    Timestamp = DateTime.Now.AddHours(-1),
                    IPAddress = "192.168.1.1",
                    UserId = 5,
                    User = null!
                },
                new AuditTrail
                {
                    Id = 2,
                    ActionDescription = "Plan upgraded to Pro",
                    Timestamp = DateTime.Now.AddMinutes(-30),
                    IPAddress = "10.0.0.5",
                    UserId = 12,
                    User = null!
                },
                new AuditTrail
                {
                    Id = 3,
                    ActionDescription = "Failed login attempt",
                    Timestamp = DateTime.Now.AddMinutes(-5),
                    IPAddress = "172.16.0.4",
                    UserId = 8,
                    User = null!
                },
            };
            return View(logs);
        }

        // Allows admin to configure subscription plans and modules
        public IActionResult ManagePlans()
        {
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan { Id = 1, PlanName = "Basic", MonthlyPrice = 19.99m, MaxUsers = 5, IsActive = true },
                new SubscriptionPlan { Id = 2, PlanName = "Pro", MonthlyPrice = 49.99m, MaxUsers = 20, IsActive = true },
                new SubscriptionPlan { Id = 3, PlanName = "Enterprise", MonthlyPrice = 99.99m, MaxUsers = 100, IsActive = true }
            };

            return View(plans);
        }
    }
}
