using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    // Represents a specific feature or module of the SAAS platform (e.g., HR, Analytics).
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Module name must be provided.")]
        [StringLength(50, ErrorMessage = "Module name is too long. Max 50 characters.")]
        public required string ModuleName { get; set; }

        [Required(ErrorMessage = "Please provide a short description for this module.")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public required string Description { get; set; }

        // Navigation property for Many-to-Many relationship
        public virtual ICollection<PlanModule>? PlanModules { get; set; }
    }
}
