using System;

namespace Company_Registration_API.Models
{
    public class CompanyShareCapitalDTO
    {
        public decimal AuthorizedShareCapital { get; set; }
        public decimal IssuedShareCapital { get; set; }
        public decimal PaidUpShareCapital { get; set; }
        public string ShareCurrency { get; set; } // MMK, USD
        public DateTime CreatedAt { get; set; }
    }
}