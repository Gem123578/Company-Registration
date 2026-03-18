using Company_Registration_API.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.CompanyRegistration
{
    public class ResCompanyRegistration : BaseResponse
    {
        [JsonProperty ("data")]
        public string Data { get; set; }
    }
}