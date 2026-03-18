using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.DTO
{
    public class CompanyAplicantDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}