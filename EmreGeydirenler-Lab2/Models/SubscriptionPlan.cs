using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    // Defines the subscription tiers available in the SAAS platform (e.g., Basic, Pro).
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Plan name is required (e.g., Basic, Enterprise).")]
        [StringLength(50, ErrorMessage = "Plan name cannot exceed 50 characters.")]
        [Display(Name = "Plan Name")]
        public required string PlanName { get; set; }

        [Required(ErrorMessage = "Please enter the monthly price for this plan.")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than 0 and realistic.")]
        [Display(Name = "Monthly Price ($)")]
        public decimal MonthlyPrice { get; set; }

        [Required(ErrorMessage = "Maximum users limit must be defined.")]
        [Range(1, 10000, ErrorMessage = "Max users must be at least 1.")]
        [Display(Name = "Maximum Allowed Users")]
        public int MaxUsers { get; set; }

        public bool IsActive { get; set; } = true;

        // 1-to-Many with Subscriptions
        public virtual ICollection<Subscription>? Subscriptions { get; set; }

        // Many-to-Many relationship with Modules (via PlanModule junction table)
        public virtual ICollection<PlanModule>? PlanModules { get; set; }
    }
}
