using Company_Registration_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.CompanyRegistration.Response
{
    public class UploadResponse : BaseResponse  
    {
        public string Path { get; set; }
    }
}