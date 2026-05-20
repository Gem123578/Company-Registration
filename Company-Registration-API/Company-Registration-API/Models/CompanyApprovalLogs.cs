using System;

namespace Company_Registration_API.Models
{
    public class CompanyApprovalLogs
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public DateTime ActionDate { get; set; }
        public long ApprovedBy { get; set; }
    }
}