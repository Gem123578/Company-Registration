using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class ApplicantRegisterDTO
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
    }
}