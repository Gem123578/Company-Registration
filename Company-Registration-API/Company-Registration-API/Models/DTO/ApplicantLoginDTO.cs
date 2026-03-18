using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class ApplicantLoginDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}