using System;

namespace Company_Registration_API.Models.DTO
{
    public class CompanyApplicantDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}