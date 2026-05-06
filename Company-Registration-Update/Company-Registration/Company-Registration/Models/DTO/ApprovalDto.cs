using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration.Models.DTO
{
    public class ApprovalDto
    {
        public long CompanyId { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public DateTime ActionDate { get; set; }
        public string ApprovedByName { get; set; }
    }
}