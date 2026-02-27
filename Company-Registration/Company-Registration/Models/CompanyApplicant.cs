using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Company_Registration.Models
{
    public class CompanyApplicant
    {
        public long Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}