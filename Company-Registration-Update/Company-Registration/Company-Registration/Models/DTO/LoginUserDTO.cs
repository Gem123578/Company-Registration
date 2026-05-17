using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration.Models.DTO
{
    public class LoginUserDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public List<string> Functions { get; set; }

    }
}