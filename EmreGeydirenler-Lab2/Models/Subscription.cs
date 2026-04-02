using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreGeydirenler_Lab2.Models
{
    // Tracks the active or past subscriptions of a specific customer.
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start date is strictly required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required to track expiration.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Subscription status is required (e.g., Active, Expired).")]
        public required string Status { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "A subscription must belong to a customer.")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual required Customer Customer { get; set; }

        [Required(ErrorMessage = "A subscription must be linked to a specific plan.")]
        public int PlanId { get; set; }

        [ForeignKey("PlanId")]
        public virtual required SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
