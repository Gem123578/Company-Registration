using System;

namespace Company_Registration_API.Models.ApprovalLog
{
    public class ApprovalLogRes
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public DateTime ActionDate { get; set; }
        public string ApprovedByName { get; set; }//join from SystemUsers
    }
}