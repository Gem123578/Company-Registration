using System;

namespace Company_Registration.Models
{
    public class CompanyApprovalLogViewModel
    {
        public long CompanyId { get; set; }
        public long ApprovedBy { get; set; }
        public string Action { get; set; } // APPROVED / REJECTED
        public string Remarks { get; set; }
        public DateTime ActionDate { get; set; }
    }
}