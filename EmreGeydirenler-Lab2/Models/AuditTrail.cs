using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreGeydirenler_Lab2.Models
{
    // Ensures strict "transparency and accountability" by logging every critical action.
    public class AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Action description is required for accountability.")]
        public required string ActionDescription { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "IP Address must be recorded for security reasons.")]
        public required string IPAddress { get; set; }

        // Links to BaseUser to capture actions from both Admins and Customers
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual required BaseUser User { get; set; }
    }
}
