using Company_Registration_API.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.CompanyApplicant
{
    public class RegisterResponse : ResultBase
    {
        [JsonProperty("data")]
        public CompanyAplicantDto Data { get; set; }
    }
}