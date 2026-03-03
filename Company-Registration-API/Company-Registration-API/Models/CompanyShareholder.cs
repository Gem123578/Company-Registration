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

        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        [Required]
        [StringLength(255)]
        public string ShareholderName { get; set; }

        [Required]
        [StringLength(50)]
        public string ShareholderType { get; set; } // INDIVIDUAL, CORPORATE

        [Required]
        [StringLength(100)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(100)]
        public string IdentityNumber { get; set; }

        [Required]
        public int NumberOfShares { get; set; }

        [Required]
        public decimal SharePercentage { get; set; }

        [StringLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}