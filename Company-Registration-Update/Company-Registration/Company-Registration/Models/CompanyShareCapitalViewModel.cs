using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class CompanyShareCapitalViewModel
    {
        [Display(Name = "Authorized Share Capital")]
        public decimal AuthorizedShareCapital { get; set; }

        [Display(Name = "Issued Share Capital")]
        public decimal IssuedShareCapital { get; set; }

        [Display(Name = "Paid-Up Share Capital")]
        public decimal PaidUpShareCapital { get; set; }

        [Display(Name = "Share Currency")]
        public string ShareCurrency { get; set; }
    }
}