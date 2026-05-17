using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class Users
    {
        public long Id { get; set; }
        public long? ApplicantId { get; set; }
        public long? SystemId { get; set; }
        public long RoleId { get; set; }
        public bool IsUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}