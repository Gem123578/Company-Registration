using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class CompanyShareCapitalViewModel
    {
        [Required(ErrorMessage = "Authorized Share Capital is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Authorized Share Capital must be greater than or equal to 0.")]
        [Display(Name = "Authorized Share Capital")]
        public decimal AuthorizedShareCapital { get; set; }

        [Required(ErrorMessage = "Issued Share Capital is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Issued Share Capital must be greater than or equal to 0.")]
        [Display(Name = "Issued Share Capital")]
        public decimal IssuedShareCapital { get; set; }

        [Required(ErrorMessage = "Paid-Up Share Capital is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Paid-Up Share Capital must be greater than or equal to 0.")]
        [Display(Name = "Paid-Up Share Capital")]
        public decimal PaidUpShareCapital { get; set; }

        [Required(ErrorMessage = "Share Currency is required.")]
        [Display(Name = "Share Currency")]
        public string ShareCurrency { get; set; }
    }
}