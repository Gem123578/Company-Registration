using System;

namespace Company_Registration_API.Models
{
    public class CompanyConstitutionDTO
    {
        public string ConstitutionType { get; set; } // MODEL, CUSTOM
        public string ConstitutionFilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}