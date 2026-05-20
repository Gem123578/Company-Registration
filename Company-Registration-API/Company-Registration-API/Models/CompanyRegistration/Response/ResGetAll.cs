using Company_Registration_API.Models.CompanyApplicant;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Company_Registration_API.Models.CompanyRegistration.Response
{
    public class ResGetAll : ResultBase
    {
        [JsonProperty("data")]
        public List<CompanyRegistrationDTO> Data { get; set; }
    }
}