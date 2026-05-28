using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using System.Data.Entity;

namespace Company_Registration_API.DataAccess
{
    public class ApplicantDbContext : DbContext
    {
        public ApplicantDbContext() : base("DbConnection")
        {
        }
        public DbSet<SystemUsers> SystemUsers { get; set; }
        public DbSet<CompanyApplicants> CompanyApplicants { get; set; }
        public DbSet<CompanyApprovalLogs> CompanyApprovalLogs { get; set; }
        public DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; set; }
        public DbSet<RegisteredCompany> RegisteredCompanies { get; set; }
        public DbSet<CompanyShareCapital> CompanyShareCapital { get; set; }
        public DbSet<CompanyShareholder> CompanyShareholders { get; set; }
        public DbSet<CompanyStakeholder> CompanyStakeholders { get; set; }
        public DbSet<UltimateHoldingCompany> UltimateHoldingCompanies { get; set; }
        public DbSet<CompanyConstitution> CompanyConstitutions { get; set; }
        public DbSet<RegistrationPayment> RegistrationPayments { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<RolesFunctions> RolesFunctions { get; set; }
        public DbSet<Functions> Functions { get; set; }
        public DbSet<Roles> Roles { get; set; }

    }
}