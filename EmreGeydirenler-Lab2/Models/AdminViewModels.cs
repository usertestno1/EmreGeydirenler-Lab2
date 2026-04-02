using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmreGeydirenler_Lab2.Models
{
    public class AdminCustomerListItemViewModel
    {
        public int CustomerId { get; set; }
        public required string FullName { get; set; }
        public required string CompanyName { get; set; }
        public required string CurrentActivePlan { get; set; }
        public bool HasUnpaidInvoices { get; set; }
    }

    public class AdminCustomerInvoiceViewModel
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class AdminCustomerEditViewModel
    {
        public int Id { get; set; }

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

        [Required(ErrorMessage = "Company Name is required for billing and invoice purposes.")]
        [StringLength(100, ErrorMessage = "Company Name cannot be longer than 100 characters.")]
        [Display(Name = "Company Name")]
        public required string CompanyName { get; set; }

        [Required(ErrorMessage = "Tax Number is strictly required for generating legal invoices.")]
        [StringLength(20, ErrorMessage = "Tax Number is too long. Maximum 20 characters allowed.")]
        [Display(Name = "Tax Number")]
        public required string TaxNumber { get; set; }

        [Required(ErrorMessage = "Address information is required for the profile.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public required string Address { get; set; }
    }

    public class AdminCreateCustomerViewModel
    {
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

        [Required(ErrorMessage = "Password is required for customer login.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Company Name is required for billing and invoice purposes.")]
        [StringLength(100, ErrorMessage = "Company Name cannot be longer than 100 characters.")]
        [Display(Name = "Company Name")]
        public required string CompanyName { get; set; }

        [Required(ErrorMessage = "Tax Number is strictly required for generating legal invoices.")]
        [StringLength(20, ErrorMessage = "Tax Number is too long. Maximum 20 characters allowed.")]
        [Display(Name = "Tax Number")]
        public required string TaxNumber { get; set; }

        [Required(ErrorMessage = "Address information is required for the profile.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public required string Address { get; set; }
    }

    public class AdminUserListItemViewModel
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
