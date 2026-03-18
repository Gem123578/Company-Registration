using Company_Registration.Common;
using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class UltimateHoldingCompanyViewModel
    {
        [Display(Name = "UHC Name")]
        public string UHCName { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Country of Incorporation")]
        public Country CountryOfIncorporation { get; set; }

        [Display(Name = "Ownership Percentage")]
        public decimal OwnershipPercentage { get; set; }
    }
}