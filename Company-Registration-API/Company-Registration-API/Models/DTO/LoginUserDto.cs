using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.DTO
{
    public class LoginUserDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}