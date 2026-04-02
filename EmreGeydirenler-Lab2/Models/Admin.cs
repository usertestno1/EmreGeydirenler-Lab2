using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    // Admin inherits from BaseUser. Represents QuTech internal staff members.
    public class Admin : BaseUser
    {
        [Required(ErrorMessage = "Please specify the admin role (e.g., SuperAdmin, Support).")]
        [StringLength(30, ErrorMessage = "Role name cannot exceed 30 characters.")]
        [Display(Name = "Administrator Role")]
        public required string AdminRole { get; set; }

        [Required(ErrorMessage = "Department name is required to assign internal tasks correctly.")]
        [StringLength(50, ErrorMessage = "Department name must be under 50 characters.")]
        public required string Department { get; set; }
    }
}
