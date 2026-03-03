using Company_Registration_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Resources;
using System.Web;

namespace Company_Registration_API.DataAccess
{
    public class ApplicantDbContext : DbContext
    {
        public ApplicantDbContext() : base("DbConnection")
        {
        }

        public DbSet<CompanyApplicant> CompanyApplicants { get; set; }
        public DbSet<RegisteredCompany> RegisteredCompanies { get; set; }
        public DbSet<CompanyShareCapital> CompanyShareCapital { get; set; }
        public DbSet<CompanyShareholder> CompanyShareholders { get; set; }
        public DbSet<CompanyStakeholder> CompanyStakeholders { get; set; }
        public DbSet<UltimateHoldingCompany> UltimateHoldingCompanies { get; set; }
        public DbSet<CompanyConstitution> CompanyConstitutions { get; set; }
        public DbSet<RegistrationPayment> RegistrationPayments { get; set; }

    }
}