using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company_Registration_API.Models
{
    [Table("CompanyConstitutions")]
    public class CompanyConstitution
    {
        [Key]
        public long Id { get; set; }

        
        public long CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual RegisteredCompany RegisteredCompany { get; set; }

        
        [StringLength(50)]
        public string ConstitutionType { get; set; } // MODEL, CUSTOM

        
        [StringLength(500)]
        public string ConstitutionFilePath { get; set; }

       
        public DateTime UploadedAt { get; set; }
    }
}