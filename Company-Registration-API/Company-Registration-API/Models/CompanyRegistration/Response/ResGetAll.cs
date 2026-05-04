using Company_Registration_API.Models.CompanyApplicant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.CompanyRegistration.Response
{
    public class ResGetAll : ResultBase
    {
        [JsonProperty("data")]
        public List<CompanyRegistrationDTO> Data { get; set; }
    }
}