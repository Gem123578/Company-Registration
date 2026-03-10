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

       
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        
        [StringLength(100)]
        public string TransactionId { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; } // CARD, BANK_TRANSFER

       
        public decimal Amount { get; set; }

        
        [StringLength(10)]
        public string CurrencyCode { get; set; }

        
        [StringLength(50)]
        public string PaymentStatus { get; set; } // PAID, FAILED, PENDING

        
        public DateTime PaidAt { get; set; }
    }
}
