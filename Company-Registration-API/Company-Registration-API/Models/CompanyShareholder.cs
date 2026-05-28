using Company_Registration_API.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("CompanyShareholders")]
    public class CompanyShareholder
    {
        [Key]
        public long Id { get; set; }

       
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        
        public string ShareholderName { get; set; }

        public EnumCollection.ShareholderTypeEnum ShareholderType { get; set; } // INDIVIDUAL, CORPORATE

     
        public string Nationality { get; set; }

        public string IdentityNumber { get; set; }

       
        public int NumberOfShares { get; set; }

        
        public decimal SharePercentage { get; set; }

        public string EmailAddress { get; set; }

        
        public DateTime CreatedAt { get; set; }
    }
}