using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{

    [Table("RegisteredCompanies")]
    public class RegisteredCompany
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public EnumCollection.CompanyTypeEnum CompanyType { get; set; }
        public string BusinessActivity { get; set; }
        public string RegisteredAddress { get; set; }
        public EnumCollection.RegistrationStatusEnum RegistrationStatus { get; set; }
        public long? ApplicantId { get; set; }
        public long? UserId { get; set; }
        public DateTime IncorporationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<CompanyShareCapital> CompanyShareCapital { get; set; }
        public virtual ICollection<CompanyShareholder> CompanyShareholders { get; set; }
        public virtual ICollection<CompanyStakeholder> CompanyStakeholders { get; set; }
        public virtual ICollection<UltimateHoldingCompany> UltimateHoldingCompanies { get; set; }
        public virtual ICollection<CompanyConstitution> CompanyConstitutions { get; set; }
        public virtual ICollection<RegistrationPayment> RegistrationPayments { get; set; }
    }
}