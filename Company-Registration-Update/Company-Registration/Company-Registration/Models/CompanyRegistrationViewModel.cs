using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Company_Registration.Models
{
    public class CompanyRegistrationViewModel
    {
        // ================== RegisteredCompanies ==================
        public long? CompanyId { get; set; }

        //[Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        //[Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        //[Required]
        [Display(Name = "Company Type")]
        public string CompanyType { get; set; } // LLC, PLC, Partnership

        //[Required]
        [Display(Name = "Business Activity")]
        public string BusinessActivity { get; set; }

        //[Required]
        [Display(Name = "Registered Address")]
        public string RegisteredAddress { get; set; }

        //[Required]
        [Display(Name = "Incorporation Date")]
        public DateTime IncorporationDate { get; set; }

        //[Required]
        [Display(Name = "Registration Status")]
        public string RegistrationStatus { get; set; } = "PENDING";

        public long ApplicantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ================== Share Capital ==================
        public CompanyShareCapitalViewModel ShareCapital { get; set; }

        // ================== Shareholders ==================
        public List<CompanyShareholderViewModel> Shareholders { get; set; } 

        // ================== Ultimate Holding Company ==================
        public UltimateHoldingCompanyViewModel UHC { get; set; } 

        // ================== Company Constitution ==================
        public CompanyConstitutionViewModel Constitution { get; set; } 

        // ================== Payment Info ==================
        public RegistrationPaymentViewModel Payment { get; set; } 

        // ================== Approval Logs ==================
        public List<CompanyApprovalLogViewModel> ApprovalLogs { get; set; } 
    }
}