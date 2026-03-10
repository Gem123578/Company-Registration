using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Company_Registration.Models
{
    public class CompanyApplicantViewModel
    {
        public long Id { get; set; }  // Database primary key

        [Required]
        [StringLength(255)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //[Required]
        //[StringLength(255)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        [Phone]
        [StringLength(30)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}