using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

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