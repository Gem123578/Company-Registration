using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class CompanyShareholderViewModel
    {
        //[Required]
        [Display(Name = "Shareholder Name")]
        public string ShareholderName { get; set; }

        //[Required]
        [Display(Name = "Shareholder Type")]
        public string ShareholderType { get; set; } // INDIVIDUAL / CORPORATE

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Number of Shares")]
        public int NumberOfShares { get; set; }

        [Display(Name = "Share Percentage")]
        public decimal SharePercentage { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}