using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(15, ErrorMessage = "Phone Number cannot be longer than 15 characters.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Restaurant Name is required")]
        [StringLength(150, ErrorMessage = "Restaurant Name cannot be longer than 150 characters.")]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Contact Person is required")]
        [StringLength(100, ErrorMessage = "Contact Person cannot be longer than 100 characters.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Contact Phone is required")]
        [Phone(ErrorMessage = "Invalid Contact Phone Number")]
        [StringLength(15, ErrorMessage = "Contact Phone cannot be longer than 15 characters.")]
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }
        [Required(ErrorMessage = "Area is required")]
        [StringLength(100, ErrorMessage = "Area cannot be longer than 100 characters.")]
        [Display(Name = "Area")]
        public string Area { get; set; }
    }
}