using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.SystemUser
{
    public class ResLoginSystemUser : ResultBase
    {
        [JsonProperty("data")]
        public SystemUsers Data { get; set; }
    }
}