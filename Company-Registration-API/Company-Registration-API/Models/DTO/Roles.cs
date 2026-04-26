using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.DTO
{
    public class Roles
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<SystemUsers> SystemUsers { get; set; }
    }
}