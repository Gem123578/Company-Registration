using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Company_Registration_API.Models
{
    public class EmailConfirmationToken
    {
        public long Id { get; set; }
        public long ApplicantId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("ApplicantId")]
        [JsonIgnore]
        public virtual CompanyApplicants Applicant { get; set; }
    }
}