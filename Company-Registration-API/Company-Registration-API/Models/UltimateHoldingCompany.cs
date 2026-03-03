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

        [Required]
        [StringLength(255)]
        public string UHCName { get; set; }

        [Required]
        [StringLength(100)]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryOfIncorporation { get; set; }

        [Required]
        public decimal OwnershipPercentage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}