using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreGeydirenler_Lab2.Models
{
    // Handles the billing process. A single customer will receive multiple invoices over time.
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Issue date must be specified for the invoice.")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Due date must be set for payment tracking.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Total amount cannot be empty.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        public bool IsPaid { get; set; } = false;

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual required Customer Customer { get; set; }
    }
}
