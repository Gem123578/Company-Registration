using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class LoginDTO 
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}