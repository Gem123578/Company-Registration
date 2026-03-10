using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration.Models
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}