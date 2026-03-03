using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("CompanyStakeholders")]
    public class CompanyStakeholder
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string StakeholderRole { get; set; } // DIRECTOR, SHAREHOLDER, SECRETARY

        [Required]
        public decimal SharePercentage { get; set; }

        [Required]
        [StringLength(100)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(100)]
        public string IdentityNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; }
    }
}