using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    // Customer inherits from BaseUser. It represents the clients using our QuTech SAAS platform.
    public class Customer : BaseUser
    {
        [Required(ErrorMessage = "Company Name is required for billing and invoice purposes.")]
        [StringLength(100, ErrorMessage = "Company Name cannot be longer than 100 characters.")]
        [Display(Name = "Company Name")]
        public required string CompanyName { get; set; }

        [Required(ErrorMessage = "Tax Number is strictly required for generating legal invoices.")]
        [StringLength(20, ErrorMessage = "Tax Number is too long. Maximum 20 characters allowed.")]
        [Display(Name = "Tax Number")]
        public required string TaxNumber { get; set; }

        // Navigation properties to define complex entity relationships

        // 1-to-1 relationship with AccountSetting
        public virtual AccountSetting? AccountSetting { get; set; }

        // 1-to-Many relationships
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Subscription>? Subscriptions { get; set; }
        public virtual ICollection<UsageRecord>? UsageRecords { get; set; }
    }
}
