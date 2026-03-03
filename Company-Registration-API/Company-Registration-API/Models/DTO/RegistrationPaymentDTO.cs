using System;

namespace Company_Registration_API.Models
{
    public class RegistrationPaymentDTO
    {
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; } // CARD, BANK_TRANSFER
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentStatus { get; set; } // PAID, FAILED, PENDING
        public DateTime PaidAt { get; set; }
    }
}