using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Company_Registration.Models
{
    public class CompanyApplicantViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Identity number is required")]
        [StringLength(30)]
        [RegularExpression(@"^\d{1,2}\/[A-Za-z]+\([A-Za-z]\)\d{6}$", ErrorMessage = "Invalid identity number format")]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}