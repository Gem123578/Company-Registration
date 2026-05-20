using System;

namespace Company_Registration_API.Models
{
    public class SystemUsers
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public bool AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}