using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Handles customer billing and invoice history.
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lists all past and current invoices for the customer
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .ThenInclude(c => c.Subscriptions)
                .ThenInclude(s => s.SubscriptionPlan)
                .AsNoTracking()
                .OrderByDescending(i => i.IssueDate)
                .ToListAsync();

            return View(invoices);
        }

        // Shows a detailed breakdown of a specific invoice
        public IActionResult Details(int id)
        {
            ViewBag.InvoiceId = id;
            ViewBag.CompanyName = "Dummy Corp";
            ViewBag.Amount = 49.99m;
            return View();
        }
    }
}
