namespace Company_Registration_API.Models
{
    public class CompanyStakeholderDTO
    {
        public string FullName { get; set; }
        public string StakeholderRole { get; set; } // DIRECTOR, SHAREHOLDER, SECRETARY
        public decimal SharePercentage { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}