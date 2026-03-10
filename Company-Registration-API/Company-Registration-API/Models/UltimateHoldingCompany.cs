using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("UltimateHoldingCompanies")]
    public class UltimateHoldingCompany
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        
        [StringLength(255)]
        public string UHCName { get; set; }

        
        [StringLength(100)]
        public string RegistrationNumber { get; set; }

        
        [StringLength(100)]
        public string CountryOfIncorporation { get; set; }

     
        public decimal OwnershipPercentage { get; set; }

        
        public DateTime CreatedAt { get; set; }
    }
}