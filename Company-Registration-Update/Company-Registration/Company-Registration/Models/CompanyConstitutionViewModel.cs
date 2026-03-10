using System.ComponentModel.DataAnnotations;

namespace Company_Registration.Models
{
    public class CompanyConstitutionViewModel
    {
        [Display(Name = "Constitution Type")]
        public string ConstitutionType { get; set; } // MODEL / CUSTOM

        [Display(Name = "File Path")]
        public string ConstitutionFilePath { get; set; }
    }
}