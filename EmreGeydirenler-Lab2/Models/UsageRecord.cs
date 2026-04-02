using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreGeydirenler_Lab2.Models
{
    // Fulfills the "usage tracking" requirement by logging resource consumption.
    public class UsageRecord
    {
        [Key]
        public int Id { get; set; }

        public DateTime LogDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "API request count must be logged.")]
        public int ApiRequestsCount { get; set; }

        [Required(ErrorMessage = "Storage usage must be logged.")]
        public decimal StorageUsedMb { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual required Customer Customer { get; set; }
    }
}
