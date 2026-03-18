using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.CompanyRegistration.Request
{
    public class ReqCompanyRegistration
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public long CompanyId { get; set; }
    }
}