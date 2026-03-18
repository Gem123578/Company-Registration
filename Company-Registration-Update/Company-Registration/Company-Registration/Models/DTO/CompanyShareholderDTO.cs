using Company_Registration.Common;
using System;

namespace Company_Registration_API.Models
{
    public class CompanyShareholderDTO
    {
        public string ShareholderName { get; set; }
        public string ShareholderType { get; set; } // INDIVIDUAL, CORPORATE
        public Country Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public int? NumberOfShares { get; set; }
        public decimal SharePercentage { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}