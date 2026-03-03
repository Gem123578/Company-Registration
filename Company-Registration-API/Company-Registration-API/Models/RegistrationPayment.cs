using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("RegistrationPayments")]
    public class RegistrationPayment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // CARD, BANK_TRANSFER

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(10)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; } // PAID, FAILED, PENDING

        [Required]
        public DateTime PaidAt { get; set; }
    }
}
