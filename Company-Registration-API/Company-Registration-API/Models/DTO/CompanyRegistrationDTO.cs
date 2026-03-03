using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class CompanyRegistrationDTO
    {
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public string CompanyType { get; set; } // LLC, PLC, Partnership
        public string BusinessActivity { get; set; }
        public string RegisteredAddress { get; set; }
        public string RegistrationStatus { get; set; } // PENDING, APPROVED, REJECTED
        public long ApplicantId { get; set; }
        public DateTime IncorporationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        // ================= Share Capital =================
        public CompanyShareCapitalDTO ShareCapital { get; set; }

        // ================= Shareholders =================
        public List<CompanyShareholderDTO> Shareholders { get; set; }

        // ================= Company Stakeholders (Directors/Secretaries) =================
        public List<CompanyStakeholderDTO> CompanyStakeholders { get; set; }

        // ================= Ultimate Holding Company =================
        public UltimateHoldingCompanyDTO UHC { get; set; }

        // ================= Company Constitution =================
        public CompanyConstitutionDTO Constitution { get; set; }

        // ================= Payment =================
        public RegistrationPaymentDTO Payment { get; set; }

    }
}