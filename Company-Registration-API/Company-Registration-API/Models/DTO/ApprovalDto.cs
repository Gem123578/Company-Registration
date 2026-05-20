using System;

namespace Company_Registration_API.Models.DTO
{
    public class ApprovalDto
    {
        public string ApprovalAction { get; set; }
        public string ApprovalRemarks { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedByName { get; set; }

    }
}