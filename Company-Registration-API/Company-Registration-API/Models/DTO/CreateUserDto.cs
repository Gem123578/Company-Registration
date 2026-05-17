using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.DTO
{
    public class CreateUserDto
    {
        public long Id { get; set; }
        public string UserName  { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
        public string AccountStatus { get; set; }

        public long RoleId { get; set; }
        public bool IsUpdate { get; set; }
        public string RoleName { get; set; }
    }
}