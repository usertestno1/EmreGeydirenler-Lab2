using System;
using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    // Abstract base class to implement inheritance for all users (Customers and Admins)
    // This prevents repeating common properties like Name, Email, and Phone.
    public abstract class BaseUser
    {
        [Key]
        public int Id { get; set; }

        // Customizing the error messages as required by the rubric instead of default ones
        [Required(ErrorMessage = "Please enter the first name. It cannot be empty.")]
        [StringLength(50, ErrorMessage = "First name is too long. Max 50 characters allowed.")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the last name.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email address is strongly required for contact and login.")]
        [EmailAddress(ErrorMessage = "The email format is invalid. Please check and try again.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required to reach the user.")]
        [Phone(ErrorMessage = "Please enter a valid phone number format.")]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address information is required for the profile.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public required string Address { get; set; }

        // Automatically set when the user registers in the system
        [Display(Name = "Registration Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
