using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreGeydirenler_Lab2.Models
{
    // Represents the security and notification settings for a specific customer.
    // We use a strict 1-to-1 relationship here for database normalization.
    public class AccountSetting
    {
        // CustomerId is both the Primary Key and Foreign Key
        [Key]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please specify if Two-Factor Authentication is enabled.")]
        [Display(Name = "Enable Two-Factor Authentication (2FA)")]
        public bool TwoFactorEnabled { get; set; } = false;

        [Required(ErrorMessage = "Please specify if you want to receive email alerts.")]
        [Display(Name = "Receive Email Alerts")]
        public bool ReceiveEmailAlerts { get; set; } = true;

        // Navigation property back to the Customer
        public virtual required Customer Customer { get; set; }
    }
}
