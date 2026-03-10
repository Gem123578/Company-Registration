using System;
using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class RegistrationPaymentViewModel
    {
        [Display(Name = "Transaction ID")]
        public string TransactionId { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } // CARD / BANK_TRANSFER

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } // PAID / FAILED / PENDING

        [Display(Name = "Paid At")]
        public DateTime PaidAt { get; set; }
    }
}