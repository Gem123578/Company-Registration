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

        
        [StringLength(255)]
        public string ShareholderName { get; set; }

       
        [StringLength(50)]
        public string ShareholderType { get; set; } // INDIVIDUAL, CORPORATE

       
        [StringLength(100)]
        public string Nationality { get; set; }

        
        [StringLength(100)]
        public string IdentityNumber { get; set; }

       
        public int NumberOfShares { get; set; }

        
        public decimal SharePercentage { get; set; }

        [StringLength(255)]
        public string EmailAddress { get; set; }

        
        public DateTime CreatedAt { get; set; }
    }
}