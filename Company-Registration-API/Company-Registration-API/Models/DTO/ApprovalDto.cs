using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.DTO
{
    public class ApprovalDto
    {
        public long UserId { get; set; } //ApprovedBy
        public string Remarks { get; set; }
    }
}