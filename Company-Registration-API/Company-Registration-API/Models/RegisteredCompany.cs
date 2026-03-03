using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{

    [Table("RegisteredCompanies")]
    public class RegisteredCompany
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public string CompanyType { get; set; }
        public string BusinessActivity { get; set; }
        public string RegisteredAddress { get; set; }
        public string RegistrationStatus { get; set; }
        public long ApplicantId { get; set; }
        public DateTime IncorporationDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<CompanyShareCapital> CompanyShareCapital { get; set; }
        public virtual ICollection<CompanyShareholder> CompanyShareholders { get; set; }
        public virtual ICollection<CompanyStakeholder> CompanyStakeholders { get; set; }
        public virtual ICollection<UltimateHoldingCompany> UltimateHoldingCompanies { get; set; }
        public virtual ICollection<CompanyConstitution> CompanyConstitutions { get; set; }
        public virtual ICollection<RegistrationPayment> RegistrationPayments { get; set; }
    }
}