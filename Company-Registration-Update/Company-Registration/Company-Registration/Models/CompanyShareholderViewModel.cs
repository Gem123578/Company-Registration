using Company_Registration.Common;
using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class CompanyShareholderViewModel
    {
        [Required(ErrorMessage = "Shareholder Name is required.")]
        [StringLength(100, ErrorMessage = "Shareholder Name cannot exceed 100 characters.")]
        [Display(Name = "Shareholder Name")]
        public string ShareholderName { get; set; }

        [Required(ErrorMessage = "Shareholder Type is required.")]
        [Display(Name = "Shareholder Type")]
        public string ShareholderType { get; set; } // INDIVIDUAL / CORPORATE

        [Required(ErrorMessage = "Nationality is required.")]
        [Display(Name = "Nationality")]
        public Country Nationality { get; set; }

        [Required(ErrorMessage = "Identity Number is required.")]
        [StringLength(50, ErrorMessage = "Identity Number cannot exceed 50 characters.")]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Number of Shares is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of Shares must be greater than 0.")]
        [Display(Name = "Number of Shares")]
        public int NumberOfShares { get; set; }

        [Required(ErrorMessage = "Share Percentage is required.")]
        [Range(0, 100, ErrorMessage = "Share Percentage must be between 0 and 100.")]
        [Display(Name = "Share Percentage")]
        public decimal SharePercentage { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}