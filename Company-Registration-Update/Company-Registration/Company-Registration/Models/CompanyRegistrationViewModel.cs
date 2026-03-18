using Company_Registration_API.Models;
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

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(200, ErrorMessage = "Company Name cannot exceed 200 characters.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        [StringLength(100, ErrorMessage = "Registration Number cannot exceed 100 characters.")]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Company Type is required.")]
        [Display(Name = "Company Type")]
        public string CompanyType { get; set; } // LLC, PLC, Partnership

        [StringLength(255, ErrorMessage = "Business Activity cannot exceed 255 characters.")]
        [Display(Name = "Business Activity")]
        public string BusinessActivity { get; set; }

        [Required(ErrorMessage = "Registered Address is required.")]
        [StringLength(300, ErrorMessage = "Registered Address cannot exceed 300 characters.")]
        [Display(Name = "Registered Address")]
        public string RegisteredAddress { get; set; }

        [Required(ErrorMessage = "Incorporation Date is required.")]
        [Display(Name = "Incorporation Date")]
        public DateTime IncorporationDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Registration Status")]
        public string RegistrationStatus { get; set; } = "PENDING";

        [Required]
        public long ApplicantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ================== Share Capital ==================
        public CompanyShareCapitalViewModel ShareCapital { get; set; }

        // ================== Shareholders ==================
        public List<CompanyShareholderViewModel> Shareholders { get; set; } 

        // ================== Stakeholders ==================
        public List<CompanyStakeholderDTO> CompanyStakeholders { get; set; }

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