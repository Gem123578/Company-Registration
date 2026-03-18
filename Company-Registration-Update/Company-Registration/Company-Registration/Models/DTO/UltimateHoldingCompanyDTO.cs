using Company_Registration.Common;
using System;

namespace Company_Registration_API.Models
{
    public class UltimateHoldingCompanyDTO
    {
        public string UHCName { get; set; }
        public string RegistrationNumber { get; set; }
        public Country CountryOfIncorporation { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}