using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models
{
    public class EmailConfirmationToken
    {
        public long TokenId { get; set; }
        public long ApplicantId { get; set; }
        public string EmailToken { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}