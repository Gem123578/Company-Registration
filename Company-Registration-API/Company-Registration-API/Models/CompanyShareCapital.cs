using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("CompanyShareCapital")]
    public class CompanyShareCapital
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        [Required]
        public decimal AuthorizedShareCapital { get; set; }

        [Required]
        public decimal IssuedShareCapital { get; set; }

        [Required]
        public decimal PaidUpShareCapital { get; set; }

        [Required]
        [StringLength(10)]
        public string ShareCurrency { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}