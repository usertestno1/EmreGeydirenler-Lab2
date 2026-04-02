using System;
using System.Collections.Generic;
using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmreGeydirenler_Lab2.Controllers
{
    // Handles customer billing and invoice history.
    public class InvoiceController : Controller
    {
        // Lists all past and current invoices for the customer
        public IActionResult Index()
        {
            // Creating dummy invoice data to display in the view table
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    Id = 1001,
                    IssueDate = DateTime.Now.AddMonths(-2),
                    DueDate = DateTime.Now.AddMonths(-2).AddDays(15),
                    TotalAmount = 49.99m,
                    IsPaid = true,
                    Customer = null!,
                },
                new Invoice
                {
                    Id = 1002,
                    IssueDate = DateTime.Now.AddMonths(-1),
                    DueDate = DateTime.Now.AddMonths(-1).AddDays(15),
                    TotalAmount = 49.99m,
                    IsPaid = true,
                    Customer = null!,
                },
                new Invoice
                {
                    Id = 1003,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    TotalAmount = 49.99m,
                    IsPaid = false,
                    Customer = null!,
                }, // Pending invoice
            };
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
